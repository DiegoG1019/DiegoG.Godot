using System.Diagnostics.CodeAnalysis;
using Godot;

namespace DiegoG.Godot.Common.Tasks;

public partial class BackgroundUpdater : Node
{
    public BackgroundUpdater() { }

    protected Task? Task;
    
    public IAsyncUpdateable? Updateable { get; set; }
    
    public override void _Process(double delta)
    {
        if (Task is Task t)
            if (CheckTaskAndAwaitIfReady(Task, delta) is false)
                return;
            else
                Task = null;

        var ta = LaunchUpdate(delta);
        if (ta is null) return;
        Task = ta;
    }
    
    // We already know the task is completed when checked
    [SuppressMessage("Usage", "VSTHRD002:Avoid problematic synchronous waits")]
    protected virtual bool CheckTaskAndAwaitIfReady(Task task, double delta)
    {
        if (task.IsCompleted)
        {
            Task = null;
            task.ConfigureAwait(false).GetAwaiter().GetResult();
            return true;
        }
        else return false;
    }
    
    // Unapplicable, as the Task is nullable.
    [SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods")]
    protected virtual Task? LaunchUpdate(double delta) 
        => Updateable is not IAsyncUpdateable upd ? null : upd.ProcessAsync(delta);
}