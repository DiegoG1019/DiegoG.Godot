using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Godot;

namespace DiegoG.Godot.Common;

public static class VectorExtensions
{
    public static Vector2 GetPositionFromDistanceAndAngle(float angleRadians, float distance)
        => new Vector2(distance * float.Cos(angleRadians), distance * float.Sin(angleRadians));

    extension(Vector3 vec)
    {
        public System.Numerics.Vector3 ToNumerics()
            => new(vec.X, vec.Y, vec.Z);
        
        public Vector3I ToVector3I()
            => new((int)vec.X, (int)vec.Y, (int)vec.Z);

        /// <summary>
        /// Takes the X and Z values from the vector and puts them into a Vector2, ignoring the vertical (Y) axis
        /// </summary>
        public Vector2 Horizontal => new Vector2(vec.X, vec.Z);
    }

    extension(Vector4 vec)
    {
        public System.Numerics.Vector4 ToNumerics()
            => new(vec.X, vec.Y, vec.Y, vec.W);
        
        public Vector4I ToVector4I()
            => new((int)vec.X, (int)vec.Y, (int)vec.Z, (int)vec.W);
    }

    extension(Vector2 vec)
    {
        public System.Numerics.Vector2 ToNumerics()
            => new(vec.X, vec.Y);

        public Vector2I ToVector2I()
            => new((int)vec.X, (int)vec.Y);
        
        /*
        /// <summary>
        /// Calculates the angle between the current vector and <paramref name="b"/>
        /// </summary>
        public float AngleBetween(Vector2 b) 
            => float.Atan2(b.Y - vec.Y, b.X - vec.X);

        /// <summary>
        /// Calculates the angle between the currenct vector and a Vector pointing upwards (x: 0, y: -1)
        /// </summary>
        public float AngleBetweenUp()
            => vec.AngleBetween(new Vector2(0, -1));
        //*/

        /// <summary>
        /// Calculates the Vector that would be ahead of <paramref name="vec"/> at a given angle and distance
        /// </summary>
        /// <remarks>
        /// This method uses <see cref="VectorExtensions.GetPositionFromDistanceAndAngle"/> and adds the original vector to the result
        /// </remarks>
        public Vector2 GetVectorAhead(float angleRadians, float distance)
            => GetPositionFromDistanceAndAngle(angleRadians, distance) + vec;

        public Vector2 RotatedAround(Vector2 origin, float angle)
        {
            var v = vec;
            v -= origin;
            (float sin, float cos) = Mathf.SinCos(angle);
            v = new Vector2(v.X * cos - v.Y * sin, v.X * sin + v.Y * cos);
            v += origin;
            return v;
        }
    }

    // these functions are so tiny it's almost unnecessary to use this attribute; but I wanna make sure nonetheless
    [MethodImpl(MethodImplOptions.AggressiveInlining)] 
    public static ulong Pack(int a, int b)
        => Pack((uint)a, (uint)b);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Pack(uint a, uint b)
        => unchecked(a | ((ulong)b << 32));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Unpack(ulong packed, out int a, out int b)
    {
        unchecked
        {
            a = (int)packed;
            b = (int)((packed) >> 32);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ToLong(this Vector2I point)
        => Pack(point.X, point.Y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2I Vector2I(this ulong value)
        => unchecked(new((int)value, (int)((value) >> 32)));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rect2I ToRectangle(this Vector4 vec)
        => new(
            (int)vec.X,
            (int)vec.Y,
            (int)vec.Z,
            (int)vec.W
        );

    extension(in Rect2 rect)
    {
        public float X => rect.Position.X;
        public float Y => rect.Position.Y;
        public float Width => rect.Size.X;
        public float Height => rect.Size.Y;
        
        public Vector2 GetTopRight(float mult)
            => new(rect.Position.X * mult + rect.Size.X * mult, rect.Position.Y * mult);

        public Vector2 GetBottomLeft(float mult)
            => new(rect.Position.X * mult, rect.Position.Y * mult + rect.Size.Y * mult);

        public Vector2 GetTopLeft(float mult)
            => new(rect.Position.X * mult, rect.Position.Y * mult);

        public Vector2 GetBottomRight(float mult)
            => new(rect.Position.X * mult + rect.Size.X * mult, rect.Position.Y * mult + rect.Size.Y * mult);

        public Vector2 TopRight
            => new(rect.Position.X + rect.Size.X, rect.Position.Y);

        public Vector2 BottomLeft
            => new(rect.Position.X, rect.Position.Y + rect.Size.Y);

        public Vector2 TopLeft
            => rect.Position;

        public Vector2 BottomRight
            => rect.End;

        public float Top
            => rect.Position.Y;
        
        public float Bottom
            => rect.Position.Y + rect.Size.Y;

        public float Left
            => rect.Position.X;
        
        public float Right
            => rect.Position.X + rect.Size.X;
        
        public Vector2 CenterTop 
            => new Vector2(rect.Position.X + rect.Size.X * 0.5f, rect.Position.Y); 
        
        public Vector2 CenterBottom 
            => new Vector2(rect.Position.X + rect.Size.X * 0.5f, rect.Position.Y + rect.Size.Y);
        
        public Vector2 CenterRight 
            => new Vector2(rect.Position.X + rect.Size.X, rect.Position.Y + rect.Size.Y * 0.5f);
        
        public Vector2 CenterLeft 
            => new Vector2(rect.Position.X, rect.Position.Y + rect.Size.Y * 0.5f);
    }

    extension(Rect2I rect)
    {
        public int X => rect.Position.X;
        public int Y => rect.Position.Y;
        public int Width => rect.Size.X;
        public int Height => rect.Size.Y;
        
        public Vector2I GetTopRight(int mult)
            => new(rect.Position.X * mult + rect.Size.X * mult, rect.Position.Y * mult);

        public Vector2I GetBottomLeft(int mult)
            => new(rect.Position.X * mult, rect.Position.Y * mult + rect.Size.Y * mult);

        public Vector2I GetTopLeft(int mult)
            => new(rect.Position.X * mult, rect.Position.Y * mult);

        public Vector2I GetBottomRight(int mult)
            => new(rect.Position.X * mult + rect.Size.X * mult, rect.Position.Y * mult + rect.Size.Y * mult);

        public Vector2I TopRight
            => new(rect.Position.X + rect.Size.X, rect.Position.Y);

        public Vector2I BottomLeft
            => new(rect.Position.X, rect.Position.Y + rect.Size.Y);

        public Vector2I TopLeft
            => rect.Position;

        public Vector2I BottomRight
            => rect.End;
        
        public float Top
            => rect.Position.Y;
        
        public float Bottom
            => rect.Position.Y + rect.Size.Y;

        public float Left
            => rect.Position.X;
        
        public float Right
            => rect.Position.X + rect.Size.X;

        public Vector2I CenterTop 
            => new Vector2I(rect.Position.X + rect.Size.X / 2, rect.Position.Y); 
        
        public Vector2I CenterBottom 
            => new Vector2I(rect.Position.X + rect.Size.X / 2, rect.Position.Y + rect.Size.Y);
        
        public Vector2I CenterRight 
            => new Vector2I(rect.Position.X + rect.Size.X, rect.Position.Y + rect.Size.Y / 2);
        
        public Vector2I CenterLeft 
            => new Vector2I(rect.Position.X, rect.Position.Y + rect.Size.Y / 2);
    }

    extension(List<Vector2I> points)
    {
        public Vector2I FindCentroid() 
        {
            int x = 0;
            int y = 0;

            foreach (var p in points)
            {
                x += p.X;
                y += p.Y;
            }
        
            return new(x / points.Count, y / points.Count);
        }

        public void SortVertices() 
        {
            // get centroid
            var center = points.FindCentroid();
            points.Sort((a, b) =>
            {
                double a1 = (double.RadiansToDegrees(double.Atan2(a.X - center.X, a.Y - center.Y)) + 360) % 360;
                double a2 = (double.RadiansToDegrees(double.Atan2(b.X - center.X, b.Y - center.Y)) + 360) % 360;
                return (int) (a1 - a2); 
            });
        }
    }
}
