using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;

namespace DiegoG.Godot.Common;

public readonly struct TypedByteBuffer<T>(int length) where T : unmanaged
{
    public static int SizeOfT => Unsafe.SizeOf<T>();
    public Span<T> ValueSpan => MemoryMarshal.Cast<byte, T>(new Span<byte>(ByteBufferUnsafe));
    public byte[] ByteBufferUnsafe { get; } = new byte[length * SizeOfT];
}
