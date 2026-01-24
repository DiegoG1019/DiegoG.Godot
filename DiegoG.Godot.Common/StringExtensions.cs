using GLV.Shared.Common.Collections;
using Godot;
using Godot.Collections;

namespace DiegoG.Godot.Common;

public static class NodeExtensions
{
    public readonly record struct EnumerateNodeTreeInfo(Node Current, int Level);
    
    public static IEnumerable<Node> EnumerateParents(this Node cn)
    {
        var p = cn;
        do
        {
            p = p.GetParent();
            yield return p;
        } while (p is not null);
    }

    public static Array<Node>? GetSiblingsAndSelf(this Node cn)
        => cn.GetParent()?.GetChildren();

    public static IEnumerable<Node> EnumerateSiblings(this Node cn)
    {
        var p = cn.GetParent();
        if (p is null) yield break;
        
        var children = p.GetChildren();
        if (children.Count == 0) yield break;
        
        foreach (var n in children)
            if (n != cn)
                yield return n; 
    }

    public static Node GetRoot(this Node cn)
    {
        ArgumentNullException.ThrowIfNull(cn);

        var pr = cn;
        var p = pr;
        while (true)
        {
            p = p.GetParent();
            if (p is null) return pr;
            pr = p;
        }
    }
    
    public static Node? FindParentWhere(this Node cn, Predicate<Node> predicate)
    {
        ArgumentNullException.ThrowIfNull(cn);
        ArgumentNullException.ThrowIfNull(predicate);
        
        var p = cn;
        do
        {
            p = p.GetParent();
            if (predicate.Invoke(p)) return p;
        } while (p is not null);

        return null;
    }
}

public static class StringExtensions
{
    public static Span<char> ToStringSpan(this Vector3 obj, Span<char> buffer, ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
    {
        int index = 0;
        buffer[index++] = 'X';
        buffer[index++] = ':';
        ThrowIfFormatFailed(obj.X.TryFormat(buffer[index..], out var written, format, provider));
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'Y';
        buffer[index++] = ':';
        ThrowIfFormatFailed(obj.Y.TryFormat(buffer[index..], out written, format, provider));
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'Z';
        buffer[index++] = ':';
        ThrowIfFormatFailed(obj.Z.TryFormat(buffer[index..], out written, format, provider));
        
        return buffer[..(index + written)];
    }

    public static bool TryToStringSpan(this Vector3 obj, Span<char> buffer, out Span<char> result,
        ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        result = buffer;
        
        int index = 0;
        buffer[index++] = 'X';
        buffer[index++] = ':';
        if (obj.X.TryFormat(buffer[index..], out var written, format, provider)) return false;
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'Y';
        buffer[index++] = ':';
        if (obj.Y.TryFormat(buffer[index..], out written, format, provider)) return false;
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'Z';
        buffer[index++] = ':';
        if (obj.Z.TryFormat(buffer[index..], out written, format, provider)) return false;
        
        result = buffer[..(index + written)];
        return true;
    }

    public static Span<char> ToStringSpan(this Vector4 obj, Span<char> buffer, ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
    {
        int index = 0;
        buffer[index++] = 'X';
        buffer[index++] = ':';
        ThrowIfFormatFailed(obj.X.TryFormat(buffer[index..], out var written, format, provider));
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'Y';
        buffer[index++] = ':';
        ThrowIfFormatFailed(obj.Y.TryFormat(buffer[index..], out written, format, provider));
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'Z';
        buffer[index++] = ':';
        ThrowIfFormatFailed(obj.Z.TryFormat(buffer[index..], out written, format, provider));
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'W';
        buffer[index++] = ':';
        ThrowIfFormatFailed(obj.W.TryFormat(buffer[index..], out written, format, provider));
        
        return buffer[..(index + written)];
    }

    public static bool TryToStringSpan(this Vector4 obj, Span<char> buffer, out Span<char> result,
        ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        result = buffer;
        
        int index = 0;
        buffer[index++] = 'X';
        buffer[index++] = ':';
        if (obj.X.TryFormat(buffer[index..], out var written, format, provider)) return false;
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'Y';
        buffer[index++] = ':';
        if (obj.Y.TryFormat(buffer[index..], out written, format, provider)) return false;
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'Z';
        buffer[index++] = ':';
        if (obj.Z.TryFormat(buffer[index..], out written, format, provider)) return false;
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'W';
        buffer[index++] = ':';
        if (obj.W.TryFormat(buffer[index..], out written, format, provider)) return false;
        
        result = buffer[..(index + written)];
        return true;
    }
    
    public static Span<char> ToStringSpan(this Vector2 obj, Span<char> buffer, ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
    {
        int index = 0;
        buffer[index++] = 'X';
        buffer[index++] = ':';
        ThrowIfFormatFailed(obj.X.TryFormat(buffer[index..], out var written, format, provider));
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'Y';
        buffer[index++] = ':';
        ThrowIfFormatFailed(obj.Y.TryFormat(buffer[index..], out written, format, provider));
        
        return buffer[..(index + written)];
    }

    public static bool TryToStringSpan(this Vector2 obj, Span<char> buffer, out Span<char> result,
        ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        result = buffer;
        
        int index = 0;
        buffer[index++] = 'X';
        buffer[index++] = ':';
        if (obj.X.TryFormat(buffer[index..], out var written, format, provider)) return false;
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'Y';
        buffer[index++] = ':';
        if (obj.Y.TryFormat(buffer[index..], out written, format, provider)) return false;
        
        result = buffer[..(index + written)];
        return true;
    }
    
    public static Span<char> ToStringSpanAsSize(this Vector2 obj, Span<char> buffer, ReadOnlySpan<char> format = default,
        IFormatProvider? provider = null)
    {
        int index = 0;
        buffer[index++] = 'W';
        buffer[index++] = ':';
        ThrowIfFormatFailed(obj.X.TryFormat(buffer[index..], out var written, format, provider));
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'H';
        buffer[index++] = ':';
        ThrowIfFormatFailed(obj.Y.TryFormat(buffer[index..], out written, format, provider));
        
        return buffer[..(index + written)];
    }

    public static bool TryToStringSpanAsSize(this Vector2 obj, Span<char> buffer, out Span<char> result,
        ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        result = buffer;
        
        int index = 0;
        buffer[index++] = 'W';
        buffer[index++] = ':';
        if (obj.X.TryFormat(buffer[index..], out var written, format, provider)) return false;
        index += written;
        buffer[index++] = ' ';
        buffer[index++] = 'H';
        buffer[index++] = ':';
        if (obj.Y.TryFormat(buffer[index..], out written, format, provider)) return false;
        
        result = buffer[..(index + written)];
        return true;
    }

    private static void ThrowIfFormatFailed(bool result)
    {
        if (result is false) 
            throw new ArgumentException("The buffer is not large enough to complete the format operation");
    }
}