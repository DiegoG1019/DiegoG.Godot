using GLV.Shared.Common;
using GLV.Shared.Common.Collections;
using GLV.Shared.Common.Text;
using Godot;
using ImGuiNET;

namespace DiegoG.Godot.Common;

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