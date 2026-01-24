using System.Numerics;

namespace DiegoG.Godot.Common;

public interface IValueInterpolator<TNumber> where TNumber : unmanaged, IFloatingPointIeee754<TNumber>
{
    public TNumber Interpolate(TNumber number, TNumber target, TNumber deltaSeconds);
}