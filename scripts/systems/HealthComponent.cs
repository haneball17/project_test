using Godot;

public partial class HealthComponent : Node
{
    [Signal]
    public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);

    [Signal]
    public delegate void DiedEventHandler();

    [Export]
    public int MaxHealth { get; set; } = 100;

    private HealthModel _healthModel = new(100);

    public int CurrentHealth => _healthModel.CurrentHealth;

    public override void _Ready()
    {
        _healthModel = new HealthModel(MaxHealth);
        EmitSignal(SignalName.HealthChanged, _healthModel.CurrentHealth, _healthModel.MaxHealth);
    }

    public void ApplyDamage(int damageAmount)
    {
        bool changed = _healthModel.ApplyDamage(damageAmount);
        if (!changed)
        {
            return;
        }

        EmitSignal(SignalName.HealthChanged, _healthModel.CurrentHealth, _healthModel.MaxHealth);

        if (_healthModel.IsDead)
        {
            EmitSignal(SignalName.Died);
        }
    }

    public void Heal(int healAmount)
    {
        bool changed = _healthModel.Heal(healAmount);
        if (!changed)
        {
            return;
        }

        EmitSignal(SignalName.HealthChanged, _healthModel.CurrentHealth, _healthModel.MaxHealth);
    }
}
