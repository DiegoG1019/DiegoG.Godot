using Godot;
using Godot.Collections;

namespace DiegoG.Godot.Common;

public static class NodeExtensions
{
    extension(Control ctrl)
    {
        public Rect2 ControlArea
            => new Rect2(ctrl.Position, ctrl.Size);
    }
    
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