using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DiegoG.Godot.Common.Tasks;

public static class LayerBits
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetLayerMask(byte layer)
    {
        Debug.Assert(layer < 32);
        return 1u << layer;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetLayerMask(byte layer1, byte layer2)
        => GetLayerMask(layer1) | GetLayerMask(layer2);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetLayerMask(byte layer1, byte layer2, byte layer3)
        => GetLayerMask(layer1) | GetLayerMask(layer2) | GetLayerMask(layer3);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetLayerMask(byte layer1, byte layer2, byte layer3, byte layer4)
        => GetLayerMask(layer1) | GetLayerMask(layer2) | GetLayerMask(layer3) | GetLayerMask(layer4);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetLayerMask(byte layer1, byte layer2, byte layer3, byte layer4, byte layer5)
        => GetLayerMask(layer1) | GetLayerMask(layer2) | GetLayerMask(layer3) | GetLayerMask(layer4) | GetLayerMask(layer5);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint GetLayerMask(params ReadOnlySpan<byte> layers)
    {
        uint x = 0;
        foreach (var t in layers)
        {
            Debug.Assert(t < 32);
            x |= 1u << t;
        }
        
        return x;
    }
}