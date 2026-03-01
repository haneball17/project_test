#nullable enable
using Godot;

public partial class EnemyController : CharacterBody3D, IDamageable
{
    private enum EnemyState
    {
        Patrol,
        Chase,
        Dead
    }

    [Export]
    public string PlayerPath { get; set; } = "";

    [Export]
    public float PatrolRadius { get; set; } = 4f;

    [Export]
    public float PatrolSpeed { get; set; } = 2.5f;

    [Export]
    public float ChaseSpeed { get; set; } = 4f;

    [Export]
    public float DetectionRange { get; set; } = 8f;

    [Export]
    public float LoseTargetRange { get; set; } = 11f;

    [Export]
    public float MoveAcceleration { get; set; } = 12f;

    private readonly RandomNumberGenerator _rng = new();

    private Node3D? _player;
    private HealthComponent? _healthComponent;
    private MeshInstance3D? _mesh;

    private Vector3 _spawnOrigin = Vector3.Zero;
    private Vector3 _patrolTarget = Vector3.Zero;
    private EnemyState _state = EnemyState.Patrol;

    public override void _Ready()
    {
        _rng.Randomize();

        _mesh = GetNodeOrNull<MeshInstance3D>("MeshInstance3D");
        _healthComponent = GetNodeOrNull<HealthComponent>("HealthComponent");
        if (_healthComponent == null)
        {
            GD.PushError("Enemy 缺少 HealthComponent，无法处理受击和死亡。");
        }
        else
        {
            _healthComponent.Died += OnDied;
            _healthComponent.HealthChanged += OnHealthChanged;
        }

        _spawnOrigin = GlobalPosition;
        _player = ResolvePlayer();
        PickPatrolTarget();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_state == EnemyState.Dead)
        {
            return;
        }

        float deltaSeconds = (float)delta;
        _player ??= ResolvePlayer();

        Vector3 targetPosition = _patrolTarget;
        if (_player != null)
        {
            float distanceToPlayer = HorizontalDistanceTo(_player.GlobalPosition);

            if (_state != EnemyState.Chase && distanceToPlayer <= DetectionRange)
            {
                _state = EnemyState.Chase;
            }
            else if (_state == EnemyState.Chase && distanceToPlayer > LoseTargetRange)
            {
                _state = EnemyState.Patrol;
                PickPatrolTarget();
            }

            if (_state == EnemyState.Chase)
            {
                targetPosition = _player.GlobalPosition;
            }
        }

        if (_state == EnemyState.Patrol && HorizontalDistanceTo(_patrolTarget) <= 0.6f)
        {
            PickPatrolTarget();
            targetPosition = _patrolTarget;
        }

        float targetSpeed = _state == EnemyState.Chase ? ChaseSpeed : PatrolSpeed;
        MoveToTarget(targetPosition, targetSpeed, deltaSeconds);
    }

    public void ReceiveDamage(int damageAmount)
    {
        if (_state == EnemyState.Dead || damageAmount <= 0 || _healthComponent == null)
        {
            return;
        }

        _healthComponent.ApplyDamage(damageAmount);
        PlayHitFeedback();
    }

    private void MoveToTarget(Vector3 targetPosition, float speed, float deltaSeconds)
    {
        Vector3 toTarget = targetPosition - GlobalPosition;
        toTarget.Y = 0f;

        Vector3 desiredDirection = toTarget.LengthSquared() > 0.01f ? toTarget.Normalized() : Vector3.Zero;
        (float vx, float vz) = MovementMath.ComputeHorizontalVelocity(
            Velocity.X,
            Velocity.Z,
            desiredDirection.X,
            desiredDirection.Z,
            speed,
            MoveAcceleration,
            deltaSeconds
        );

        Velocity = new Vector3(vx, Velocity.Y, vz);

        if (desiredDirection.LengthSquared() > 0.0001f)
        {
            LookAt(GlobalPosition + desiredDirection, Vector3.Up);
        }

        MoveAndSlide();
    }

    private void PickPatrolTarget()
    {
        float angle = _rng.RandfRange(0f, Mathf.Tau);
        float radius = _rng.RandfRange(PatrolRadius * 0.35f, PatrolRadius);
        _patrolTarget = _spawnOrigin + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
    }

    private Node3D? ResolvePlayer()
    {
        if (!string.IsNullOrWhiteSpace(PlayerPath))
        {
            return GetNodeOrNull<Node3D>(new NodePath(PlayerPath));
        }

        return GetTree().GetFirstNodeInGroup("player") as Node3D;
    }

    private float HorizontalDistanceTo(Vector3 targetPosition)
    {
        Vector3 delta = targetPosition - GlobalPosition;
        delta.Y = 0f;
        return delta.Length();
    }

    private void PlayHitFeedback()
    {
        if (_mesh == null)
        {
            return;
        }

        Vector3 maxScale = Vector3.One * 1.08f;
        Tween tween = CreateTween();
        _mesh.Scale = maxScale;
        tween.TweenProperty(_mesh, "scale", Vector3.One, 0.12f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.Out);
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0 || _state == EnemyState.Dead)
        {
            return;
        }
    }

    private void OnDied()
    {
        if (_state == EnemyState.Dead)
        {
            return;
        }

        _state = EnemyState.Dead;
        Velocity = Vector3.Zero;
        CollisionLayer = 0;
        CollisionMask = 0;
        SetPhysicsProcess(false);

        Tween tween = CreateTween();
        tween.TweenProperty(this, "scale", Vector3.Zero, 0.25f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.In);
        tween.TweenCallback(Callable.From(QueueFree));
    }
}
