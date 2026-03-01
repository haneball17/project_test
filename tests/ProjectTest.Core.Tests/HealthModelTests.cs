using Xunit;

public class HealthModelTests
{
    [Fact]
    public void Constructor_ShouldThrow_WhenMaxHealthIsNotPositive()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new HealthModel(0));
    }

    [Fact]
    public void ApplyDamage_ShouldReduceHealth_AndSetDeadState()
    {
        var model = new HealthModel(50);

        bool changed = model.ApplyDamage(20);
        Assert.True(changed);
        Assert.Equal(30, model.CurrentHealth);
        Assert.False(model.IsDead);

        changed = model.ApplyDamage(999);
        Assert.True(changed);
        Assert.Equal(0, model.CurrentHealth);
        Assert.True(model.IsDead);
    }

    [Fact]
    public void Heal_ShouldNotExceedMaxHealth_AndDeadCannotHeal()
    {
        var model = new HealthModel(40);
        model.ApplyDamage(10);

        bool changed = model.Heal(7);
        Assert.True(changed);
        Assert.Equal(37, model.CurrentHealth);

        changed = model.Heal(999);
        Assert.True(changed);
        Assert.Equal(40, model.CurrentHealth);

        model.ApplyDamage(999);
        changed = model.Heal(5);
        Assert.False(changed);
        Assert.Equal(0, model.CurrentHealth);
    }
}
