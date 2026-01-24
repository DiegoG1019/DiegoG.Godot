using System.Diagnostics;
using GLV.Shared.Common;
using Godot;

namespace DiegoG.Godot.Common.Tasks;

public static class BackgroundTasksSweeperFromNode 
{
    private static readonly Stopwatch Stopwatch = new();
    private static Task? sweep;
    
    public static CancellationToken CancellationToken { get; set; }

    public static void Sweep()
    {
        if (CancellationToken.IsCancellationRequested)
            BackgroundTaskStore.Disable();
        
        if (Stopwatch.Elapsed < TimeSpan.FromMilliseconds(500)) return;
        Stopwatch.Restart(); // We restart first, as the node should wait the time again if no tasks are pending
        
        // We check the sweep after the timer, it's fine to wait 500ms for tasks to have finished; or to check for an exception
        if (sweep is not null)
        {
            if (sweep.IsCompleted)
            {
#pragma warning disable VSTHRD002 // Task is known to be completed
                sweep.ConfigureAwait(false).GetAwaiter().GetResult();
                sweep = null;
            }
            else return; // Only return if it's not null and has not been completed
        }
        
        if (BackgroundTaskStore.CompletedSingleFireTasks == 0) return;
        sweep = BackgroundTaskStore.SweepAsync(CancellationToken);
    }
}
