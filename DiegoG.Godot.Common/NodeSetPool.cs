using System.Diagnostics;
using GLV.Shared.Common.Collections;
using Godot;

namespace DiegoG.Godot.Common;

public sealed class StopwatchPool(int capacity, int maximum, bool synchronized)
    : ObjectPool<Stopwatch>(capacity, maximum, synchronized)
{
    public static StopwatchPool Shared { get; } = new StopwatchPool(1, int.MaxValue, true);
    
    protected override Stopwatch CreateItem() => new Stopwatch();

    protected override void ResetItem(Stopwatch item) => item.Reset();
}

public sealed class NodeSetPool(int capacity, int maximum, bool synchronized) : ObjectPool<HashSet<Node>>(capacity, maximum, synchronized)
{
    public static NodeSetPool Shared { get; } = new(10, int.MaxValue, true);
    
    protected override HashSet<Node> CreateItem() 
        => new(100);

    protected override void ResetItem(HashSet<Node> item)
        => item.Clear();
}