using System;

public static class MovementMath
{
    public static (float x, float z) ComputeHorizontalVelocity(
        float currentX,
        float currentZ,
        float desiredDirectionX,
        float desiredDirectionZ,
        float targetSpeed,
        float acceleration,
        float deltaSeconds)
    {
        if (deltaSeconds <= 0f)
        {
            return (currentX, currentZ);
        }

        float desiredX = desiredDirectionX * targetSpeed;
        float desiredZ = desiredDirectionZ * targetSpeed;
        float maxDelta = Math.Max(0f, acceleration) * deltaSeconds;

        return (
            MoveToward(currentX, desiredX, maxDelta),
            MoveToward(currentZ, desiredZ, maxDelta)
        );
    }

    public static float ExponentialWeight(float sharpness, float deltaSeconds)
    {
        if (sharpness <= 0f || deltaSeconds <= 0f)
        {
            return 0f;
        }

        float weight = 1f - MathF.Exp(-sharpness * deltaSeconds);
        return Math.Clamp(weight, 0f, 1f);
    }

    private static float MoveToward(float current, float target, float maxDelta)
    {
        if (MathF.Abs(target - current) <= maxDelta)
        {
            return target;
        }

        return current + MathF.Sign(target - current) * maxDelta;
    }
}
