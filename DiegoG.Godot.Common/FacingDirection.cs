using System.Collections.Immutable;
using System.Numerics;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

namespace DiegoG.Godot.Common;

public readonly record struct FacingDirection
{
    public FacingDirection(float angleRadians)
    {
        AngleRadians = angleRadians;
        CardinalDirection = angleRadians.GetFacingDirection();
    }

    public FacingDirection(CardinalDirection direction)
    {
        CardinalDirection = direction;
        AngleRadians = direction.GetAngleRadians<float>();
    }
    
    public float AngleRadians { get; }
    public CardinalDirection CardinalDirection { get; }

    public static implicit operator FacingDirection(float angleRadians) => new(angleRadians);
    public static implicit operator FacingDirection(CardinalDirection direction) => new(direction);
    public static implicit operator float (FacingDirection direction) => direction.AngleRadians;
    public static implicit operator CardinalDirection (FacingDirection direction) => direction.CardinalDirection;
}

public static class CardinalDirectionData
{
    public static readonly ImmutableArray<CardinalDirection> CardinalDirectionValues =
        Enum.GetValues<CardinalDirection>().ToImmutableArray();

    public static Vector3 North { get; } = Vector3.Forward;
    public static Vector3 West { get; } = Vector3.Left;
    public static Vector3 South { get; } = Vector3.Back;
    public static Vector3 East { get; } = Vector3.Right;
    
    public static Vector3 NorthWest { get; } = (Vector3.Forward + Vector3.Left).Normalized();
    public static Vector3 SouthWest { get; } = (Vector3.Back + Vector3.Left).Normalized();
    public static Vector3 SouthEast { get; } = (Vector3.Back + Vector3.Right).Normalized();
    public static Vector3 NorthEast { get; } = (Vector3.Forward + Vector3.Right).Normalized();

    public static Vector2 NorthHorizontal { get; } = Vector3.Forward.Horizontal;
    public static Vector2 WestHorizontal { get; } = Vector3.Left.Horizontal;
    public static Vector2 SouthHorizontal { get; } = Vector3.Back.Horizontal;
    public static Vector2 EastHorizontal { get; } = Vector3.Right.Horizontal;
    
    public static Vector2 NorthWestHorizontal { get; } = (Vector3.Forward.Horizontal + Vector3.Left.Horizontal).Normalized();
    public static Vector2 SouthWestHorizontal { get; } = (Vector3.Back.Horizontal + Vector3.Left.Horizontal).Normalized();
    public static Vector2 SouthEastHorizontal { get; } = (Vector3.Back.Horizontal + Vector3.Right.Horizontal).Normalized();
    public static Vector2 NorthEastHorizontal { get; } = (Vector3.Forward.Horizontal + Vector3.Right.Horizontal).Normalized();
    
    public static T GetAngleRadians<T>(this CardinalDirection direction) where T : IFloatingPointIeee754<T>
        => direction switch
        {
            CardinalDirection.North => T.Zero,
            CardinalDirection.NorthWest => T.DegreesToRadians(T.CreateSaturating(45)),
            CardinalDirection.West => T.DegreesToRadians(T.CreateSaturating(90)),
            CardinalDirection.SouthWest => T.DegreesToRadians(T.CreateSaturating(135)),
            CardinalDirection.South => T.DegreesToRadians(T.CreateSaturating(180)),
            CardinalDirection.SouthEast => T.DegreesToRadians(T.CreateSaturating(225)),
            CardinalDirection.East => T.DegreesToRadians(T.CreateSaturating(270)),
            CardinalDirection.NorthEast => T.DegreesToRadians(T.CreateSaturating(315)),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    public static CardinalDirection GetFacingDirection<T>(this T angleRadians) where T : IFloatingPointIeee754<T>
        => (angleRadians % (T.Pi * T.CreateSaturating(2))) switch
        {
            // We need to check the angle to get the cardinal direction
            // Imagine 8 cones, one for each direction, all of equal size
            // From the center of each cone, the border is at the 22.5deg step. From -22.5deg, the left border of north,
            // the beginning of the north-western cone is 45deg to the right
            
            // 337.5 deg
            >= 5.8904867 => CardinalDirection.North,
            // 292.5 deg
            >= 5.105088 => CardinalDirection.NorthEast,
            // 247.5 deg
            >= 4.3196898 => CardinalDirection.East,
            // 202.5 deg
            >= 3.534292 => CardinalDirection.SouthEast,
            // 157.5 deg
            >= 2.7488935 => CardinalDirection.South,
            // 112.5 deg
            >= 1.9634954 => CardinalDirection.SouthWest,
            // 67.5 deg
            >= 1.1780972 => CardinalDirection.West,
            // 22.5 deg
            >= 0.3926991 => CardinalDirection.NorthWest,
            // From 337.5 deg to just before 22.5 deg, remember it's %'d with a full rotation
            _ => CardinalDirection.North
        };
}

/// <summary>
/// Provides the cardinal directions; ordered clockwise from North to NorthEast
/// </summary>
public enum CardinalDirection
{
    North,
    NorthWest,
    West,
    SouthWest,
    South,
    SouthEast,
    East,
    NorthEast,
}