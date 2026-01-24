using System.Numerics;
using Godot;

namespace DiegoG.Godot.Common;

public sealed partial class CurrentAndTargetValue<TNumber>(IValueInterpolator<TNumber> interpolator) : Node
    where TNumber : unmanaged, IFloatingPointIeee754<TNumber>
{
    public TNumber Current { get; set; }
    public TNumber Target { get; set; }

    public IValueInterpolator<TNumber> Interpolator
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            field = value;
        }
    } = interpolator ?? throw new ArgumentNullException(nameof(interpolator));

    public override void _Process(double delta)
    {
        Current = Interpolator.Interpolate(Current, Target,
            TNumber.CreateSaturating(delta));
    }

    public void ForceToTarget()
        => Current = Target;
}