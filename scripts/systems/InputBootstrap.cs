using Godot;

public partial class InputBootstrap : Node
{
    public override void _EnterTree()
    {
        EnsureDefaultInputMap();
    }

    private static void EnsureDefaultInputMap()
    {
        // 核心输入动作在运行时兜底注册，确保新环境首次启动即可操作。
        EnsureAction("move_left", Key.A, Key.Left);
        EnsureAction("move_right", Key.D, Key.Right);
        EnsureAction("move_up", Key.W, Key.Up);
        EnsureAction("move_down", Key.S, Key.Down);
        EnsureAction("attack", Key.J, Key.Space);
        EnsureAction("dash", Key.Shift);
        EnsureAction("interact", Key.E);
    }

    private static void EnsureAction(string actionName, params Key[] keys)
    {
        if (!InputMap.HasAction(actionName))
        {
            InputMap.AddAction(actionName, 0.5f);
        }

        foreach (Key key in keys)
        {
            if (ActionContainsKey(actionName, key))
            {
                continue;
            }

            InputMap.ActionAddEvent(actionName, new InputEventKey
            {
                Keycode = key,
                PhysicalKeycode = key
            });
        }
    }

    private static bool ActionContainsKey(string actionName, Key key)
    {
        foreach (InputEvent inputEvent in InputMap.ActionGetEvents(actionName))
        {
            if (inputEvent is InputEventKey keyEvent &&
                (keyEvent.Keycode == key || keyEvent.PhysicalKeycode == key))
            {
                return true;
            }
        }

        return false;
    }
}
