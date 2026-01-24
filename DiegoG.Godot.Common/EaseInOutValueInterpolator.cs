using System.Numerics;

namespace DiegoG.Godot.Common;

public sealed class EaseInOutValueInterpolator<TNumber> : IValueInterpolator<TNumber>
    where TNumber : unmanaged, IFloatingPointIeee754<TNumber>
{
    public TNumber Exponent { get; init; } = TNumber.CreateSaturating(2);

    public TNumber Interpolate(TNumber number, TNumber target, TNumber deltaSeconds)
        => ((number - (number - target) * Exponent * deltaSeconds));

    public static EaseInOutValueInterpolator<TNumber> Default { get; } = new();
}