using Godot;

namespace DiegoG.Godot.Common;

public static class ColorExtensions
{
    public static System.Numerics.Vector4 ToVector4(this Color color) 
        => new(color.R, color.G, color.B, color.A);
}