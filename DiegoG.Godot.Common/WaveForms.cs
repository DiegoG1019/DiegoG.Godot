using System.Numerics;
using System.Runtime.CompilerServices;

namespace DiegoG.Godot.Common;

public static class WaveForms<T> where T : ITrigonometricFunctions<T>
{
    public static T Sine(T amplitude, T time, T frequency, T horizontalPhaseRadians = default!,
        T verticalPhaseRadians = default!)
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            // If it's a class, then 'default' is null.
            // If I anotate it with a '?', then when it's a struct it will be a Nullable<T> which we don't want
            // If it's class then we already lose so many benefits that doing this check or not has little to no impact
            // And in both cases, the "if" block is optimized away; if it's a struct, this entire block is gone
            // If it's a class, this block is plucked out of here and dropped onto the main (and then only) code path
            horizontalPhaseRadians ??= T.Zero; 
            verticalPhaseRadians ??= T.Zero;
        }
        
        return (amplitude * T.Sin(T.CreateSaturating(2) * T.Pi * frequency * time + horizontalPhaseRadians)) + verticalPhaseRadians;
    }
    
    public static T SineWithAngularFrequency(T amplitude, T time, T angularFrequencyRadians, T horizontalPhaseRadians = default!, T verticalPhaseRadians = default!)
    {
        if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        {
            horizontalPhaseRadians ??= T.Zero;
            verticalPhaseRadians ??= T.Zero;
        }

        return (amplitude * T.Sin(angularFrequencyRadians * time + horizontalPhaseRadians)) + verticalPhaseRadians;
    }
}