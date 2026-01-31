namespace DiegoG.Godot.Common;

public static class ValueTimerExtensions
{
    extension(ref ValueTimer val)
    {
        public void Reset()
            => val = val.ToResetTimer();

        public void AddDelta(double delta)
            => val = val.WithAddedDelta(delta);
    }
}

/// <summary>
/// Represents a light-weight struct timer that accumulates delta time to keep time
/// </summary>
public readonly record struct ValueTimer(double AccumulatedDelta, double Target)
{
    public TimeSpan ElapsedCountedTime => TimeSpan.FromSeconds(AccumulatedDelta);
    public TimeSpan TargetTime => TimeSpan.FromSeconds(Target);

    public ValueTimer ToResetTimer() => new ValueTimer(0, Target);
    public ValueTimer WithAddedDelta(double delta) => new(AccumulatedDelta + delta, Target);
    
    public bool IsElapsed => AccumulatedDelta > Target;

    public bool CheckIfElapsed(out double overtime)
    {
        if (IsElapsed)
        {
            overtime = AccumulatedDelta - Target;
            return true;
        }

        overtime = 0;
        return false;
    }

    public bool CheckIfElapsed(out TimeSpan overtime)
    {
        if (IsElapsed)
        {
            overtime = TimeSpan.FromSeconds(AccumulatedDelta - Target);
            return true;
        }

        overtime = default;
        return false;
    }

    public static ValueTimer Create(double targetSeconds) => new(0, targetSeconds);
    public static ValueTimer Create(TimeSpan target) => new(0, target.TotalSeconds);
}