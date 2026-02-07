namespace DiegoG.Godot.Common;

public static class InterpolationHelpers
{
    /// <summary>
    /// Attempts to get an interpolation weight relative to the frame's delta, clamped between 0 and 1
    /// </summary>
    public static float CalcInterpolationWeight(double param, double delta)
        => (float)double.Clamp(param * delta, 0, 1);
}