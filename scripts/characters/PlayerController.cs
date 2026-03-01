using System.Collections.Generic;
using Godot;

public partial class PlayerController : CharacterBody3D
{
    private enum PlayerState
    {
        Idle,
        Move,
        Attack
    }

    [Export]
    public float MoveSpeed { get; set; } = 6f;

    [Export]
    public float Acceleration { get; set; } = 18f;

    [Export]
    public float Deceleration { get; set; } = 22f;

    [Export]
    public int AttackDamage { get; set; } = 20;

    [Export]
    public float AttackDuration { get; set; } = 0.2f;

    [Export]
    public float AttackCooldown { get; set; } = 0.35f;

    private Area3D? _attackArea;
    private readonly HashSet<EnemyController> _damagedEnemies = [];

    private bool _isAttackActive;
    private float _attackActiveRemaining;
    private float _attackCooldownRemaining;

    private PlayerState _state = PlayerState.Idle;

    public override void _Ready()
    {
        AddToGroup("player");

        _attackArea = GetNodeOrNull<Area3D>("AttackArea");
        if (_attackArea == null)
        {
            GD.PushWarning("Player 缺少 AttackArea，攻击命中将不可用。");
            return;
        }

        _attackArea.Monitoring = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        float deltaSeconds = (float)delta;

        UpdateAttackTimers(deltaSeconds);

        Vector2 inputVector = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        Vector3 moveDirection = new(inputVector.X, 0f, -inputVector.Y);
        if (moveDirection.LengthSquared() > 1f)
        {
            moveDirection = moveDirection.Normalized();
        }

        if (Input.IsActionJustPressed("attack") && _attackCooldownRemaining <= 0f)
        {
            StartAttack();
        }

        float acceleration = moveDirection.LengthSquared() > 0.0001f ? Acceleration : Deceleration;
        (float vx, float vz) = MovementMath.ComputeHorizontalVelocity(
            Velocity.X,
            Velocity.Z,
            moveDirection.X,
            moveDirection.Z,
            MoveSpeed,
            acceleration,
            deltaSeconds
        );

        Velocity = new Vector3(vx, Velocity.Y, vz);

        if (moveDirection.LengthSquared() > 0.0001f)
        {
            LookAt(GlobalPosition + moveDirection, Vector3.Up);
        }

        MoveAndSlide();

        if (_isAttackActive)
        {
            ResolveAttackHits();
        }

        UpdateState(moveDirection);
    }

    private void StartAttack()
    {
        _isAttackActive = true;
        _attackActiveRemaining = AttackDuration;
        _attackCooldownRemaining = AttackCooldown;
        _damagedEnemies.Clear();
    }

    private void UpdateAttackTimers(float deltaSeconds)
    {
        if (_attackCooldownRemaining > 0f)
        {
            _attackCooldownRemaining -= deltaSeconds;
        }

        if (!_isAttackActive)
        {
            return;
        }

        _attackActiveRemaining -= deltaSeconds;
        if (_attackActiveRemaining > 0f)
        {
            return;
        }

        _isAttackActive = false;
        _damagedEnemies.Clear();
    }

    private void ResolveAttackHits()
    {
        if (_attackArea == null)
        {
            return;
        }

        // 攻击持续时间内每帧扫描重叠体，并保证单次挥击只命中一次。
        foreach (Node3D body in _attackArea.GetOverlappingBodies())
        {
            if (body is not EnemyController enemy || _damagedEnemies.Contains(enemy))
            {
                continue;
            }

            enemy.ReceiveDamage(AttackDamage);
            _damagedEnemies.Add(enemy);
        }
    }

    private void UpdateState(Vector3 moveDirection)
    {
        if (_isAttackActive)
        {
            _state = PlayerState.Attack;
            return;
        }

        _state = moveDirection.LengthSquared() > 0.0001f ? PlayerState.Move : PlayerState.Idle;
    }
}
