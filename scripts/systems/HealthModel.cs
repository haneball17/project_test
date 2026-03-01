using System;

public sealed class HealthModel
{
    public int MaxHealth { get; }
    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    public HealthModel(int maxHealth)
    {
        if (maxHealth <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxHealth), "最大生命值必须大于 0。");
        }

        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public bool ApplyDamage(int amount)
    {
        if (amount <= 0 || IsDead)
        {
            return false;
        }

        CurrentHealth = Math.Max(0, CurrentHealth - amount);
        return true;
    }

    public bool Heal(int amount)
    {
        if (amount <= 0 || IsDead)
        {
            return false;
        }

        CurrentHealth = Math.Min(MaxHealth, CurrentHealth + amount);
        return true;
    }
}
