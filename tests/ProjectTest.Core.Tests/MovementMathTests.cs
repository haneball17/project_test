using Xunit;

public class MovementMathTests
{
    [Fact]
    public void ComputeHorizontalVelocity_ShouldMoveTowardTargetSpeed()
    {
        (float x, float z) velocity = MovementMath.ComputeHorizontalVelocity(
            currentX: 0f,
            currentZ: 0f,
            desiredDirectionX: 1f,
            desiredDirectionZ: 0f,
            targetSpeed: 6f,
            acceleration: 12f,
            deltaSeconds: 0.5f
        );

        Assert.Equal(6f, velocity.x, 3);
        Assert.Equal(0f, velocity.z, 3);
    }

    [Fact]
    public void ComputeHorizontalVelocity_ShouldDecelerateToZero_WhenNoInput()
    {
        (float x, float z) velocity = MovementMath.ComputeHorizontalVelocity(
            currentX: 3f,
            currentZ: -2f,
            desiredDirectionX: 0f,
            desiredDirectionZ: 0f,
            targetSpeed: 6f,
            acceleration: 4f,
            deltaSeconds: 1f
        );

        Assert.Equal(0f, velocity.x, 3);
        Assert.Equal(0f, velocity.z, 3);
    }

    [Theory]
    [InlineData(0f, 0.016f, 0f)]
    [InlineData(8f, 0f, 0f)]
    [InlineData(8f, 0.016f, 0.120147f)]
    public void ExponentialWeight_ShouldStayInValidRange(float sharpness, float deltaSeconds, float expected)
    {
        float weight = MovementMath.ExponentialWeight(sharpness, deltaSeconds);

        Assert.InRange(weight, 0f, 1f);
        Assert.Equal(expected, weight, 3);
    }
}
