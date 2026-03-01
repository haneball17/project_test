using Godot;

public partial class CameraFollowController : Camera3D
{
    [Export]
    public string TargetPath { get; set; } = "";

    [Export]
    public Vector3 Offset { get; set; } = new(0f, 7f, 9f);

    [Export]
    public float PositionSharpness { get; set; } = 8f;

    [Export]
    public float LookSharpness { get; set; } = 10f;

    private Node3D? _target;
    private Vector3 _lookAtPoint = Vector3.Zero;

    public override void _Ready()
    {
        _target = ResolveTarget();
        if (_target == null)
        {
            return;
        }

        _lookAtPoint = _target.GlobalPosition;
        GlobalPosition = _target.GlobalPosition + Offset;
        LookAt(_lookAtPoint, Vector3.Up);
    }

    public override void _PhysicsProcess(double delta)
    {
        _target ??= ResolveTarget();
        if (_target == null)
        {
            return;
        }

        float deltaSeconds = (float)delta;
        Vector3 desiredPosition = _target.GlobalPosition + Offset;
        Vector3 desiredLookAt = _target.GlobalPosition + Vector3.Up * 1.2f;

        float positionWeight = MovementMath.ExponentialWeight(PositionSharpness, deltaSeconds);
        float lookWeight = MovementMath.ExponentialWeight(LookSharpness, deltaSeconds);

        GlobalPosition = GlobalPosition.Lerp(desiredPosition, positionWeight);
        _lookAtPoint = _lookAtPoint.Lerp(desiredLookAt, lookWeight);
        LookAt(_lookAtPoint, Vector3.Up);
    }

    private Node3D? ResolveTarget()
    {
        if (!string.IsNullOrWhiteSpace(TargetPath))
        {
            return GetNodeOrNull<Node3D>(new NodePath(TargetPath));
        }

        return GetTree().GetFirstNodeInGroup("player") as Node3D;
    }
}
