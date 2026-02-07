using GLV.Shared.Common;
using GLV.Shared.Common.Text;
using Godot;
using ImGuiNET;

namespace DiegoG.Godot.Common;

public static class ImGuiHelpers
{
    public static void PrintImGuiTreeOrNull(this IImGuiExplorable? explorable, ReadOnlySpan<char> fmt)
    {
        if (explorable is null)
        {
            ImGui.TextColored(Colors.Orange.ToVector4(), "Object is null");
            ImGui.SameLine();
            ImGui.Text(fmt);
            return;
        }

        if (ImGui.TreeNode(fmt))
        {
            explorable.PrintImGui();
            ImGui.TreePop();
        }
    }

    public static void PrintImGuiOrNull(this IImGuiExplorable? explorable)
    {
        if (explorable is null)
        {
            ImGui.TextColored(Colors.Orange.ToVector4(), "Object is null");
            return;
        } 
        
        explorable.PrintImGui();
    }
    
    public static void PrintImGui(this ref Vector2 vector, ReadOnlySpan<char> label)
    {
        var vec = vector.ToNumerics();
        if (ImGui.InputFloat2(label, ref vec))
            vector = new Vector2(vec.X, vec.Y);
    }
    
    public static void PrintImGui(this ref Transform2D trans)
    {
        trans.X.PrintImGui("X");
        trans.Y.PrintImGui("Y");
        trans.Origin.PrintImGui("Origin");
    }

    public static void PrintImGui(this ref Vector3 vector, ReadOnlySpan<char> label)
    {
        var vec = vector.ToNumerics();
        if (ImGui.InputFloat3(label, ref vec))
            vector = new Vector3(vec.X, vec.Y, vec.Z);
    }
    
    public static void PrintImGui(this ref Basis basis)
    {
        var x = basis.X;
        var y = basis.Y;
        var z = basis.Z;
        
        x.PrintImGui("X");
        y.PrintImGui("Y");
        z.PrintImGui("Z");

        basis = new Basis(x, y, z);
    }
    
    public static void PrintImGui(this ref Transform3D trans)
    {
        trans.Basis.PrintImGui();
        trans.Origin.PrintImGui("Origin");
    }
    public static void PrintNodeToImGui(this Node? node, uint childEnumerationDepth = uint.MaxValue)
    {
        if (node is null)
        {
            ImGui.TextColored(Colors.Orange.ToVector4(), "Node is null");
            return;
        } 

        Span<char> bf = stackalloc char[1024];
        ValueStringBuilder sb = new(bf);
        sb.Append(node.Name);
        sb.Append('|');
        sb.Append(node.GetType().Name);

        if (ImGui.TreeNode(bf[..sb.Length]))
        {
            if (node is IImGuiExplorable expl)
            {
                ImGui.SeparatorText("Debug Info");
                expl.PrintImGui();
                ImGui.SeparatorText("Node Info");
            }

            ImGui.TextWrapped(node.EditorDescription);
            ImGui.LabelText("Scene File Path", node.SceneFilePath);
            ImGui.LabelText("Physics interpolation mode", Enum.GetName(node.PhysicsInterpolationMode));
            ImGui.LabelText("Process Mode", Enum.GetName(node.ProcessMode));
            ImGui.LabelText("Process Priority", node.ProcessPriority.ToStringSpan(bf));
            ImGui.LabelText("Process Thread Group", Enum.GetName(node.ProcessThreadGroup));
            ImGui.LabelText("Process Thread Group Order", node.ProcessThreadGroupOrder.ToStringSpan(bf));
            ImGui.LabelText("Process Physics Priority", node.ProcessPhysicsPriority.ToStringSpan(bf));
            ImGui.LabelText("Unique Name In Owner", node.UniqueNameInOwner ? "true" : "false");
            
            if (node is Node3D n3d)
            {
                bool tl = n3d.TopLevel;
                if (ImGui.Checkbox("Top Level", ref tl)) n3d.TopLevel = tl;

                bool vi = n3d.Visible;
                if (ImGui.Checkbox("Visible", ref vi)) n3d.Visible = vi;
                
                if (ImGui.TreeNode("Transform"))
                {
                    var trans = n3d.Transform;
                    trans.PrintImGui();
                    n3d.Transform = trans;
                    ImGui.TreePop();
                }
                
                if (ImGui.TreeNode("Transform Global"))
                {
                    var trans = n3d.GlobalTransform;
                    trans.PrintImGui();
                    n3d.Transform = trans;
                    ImGui.TreePop();
                }
            }
            else if (node is Node2D n2d)
            {
                ImGui.LabelText("Clip Children", Enum.GetName(n2d.ClipChildren));
                
                bool tl = n2d.TopLevel;
                if (ImGui.Checkbox("Top Level", ref tl)) n2d.TopLevel = tl;

                bool vi = n2d.Visible;
                if (ImGui.Checkbox("Visible", ref vi)) n2d.Visible = vi;
                
                if (ImGui.TreeNode("Transform"))
                {
                    var trans = n2d.Transform;
                    trans.PrintImGui();
                    n2d.Transform = trans;
                    ImGui.TreePop();
                }
                
                if (ImGui.TreeNode("Transform Global"))
                {
                    var trans = n2d.GlobalTransform;
                    trans.PrintImGui();
                    n2d.Transform = trans;
                    ImGui.TreePop();
                }
            }
            
            if (childEnumerationDepth > 0 && ImGui.TreeNode("Children"))
            {
                foreach (var cn in node.GetChildren()) 
                    cn.PrintNodeToImGui(childEnumerationDepth - 1);
                ImGui.TreePop();
            }
            
            ImGui.TreePop();
        }
    }
}