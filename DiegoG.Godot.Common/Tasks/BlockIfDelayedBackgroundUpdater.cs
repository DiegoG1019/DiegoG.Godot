using System.Diagnostics.CodeAnalysis;
using Godot;

namespace DiegoG.Godot.Common.Tasks;

public partial class BlockIfDelayedBackgroundUpdater : BackgroundUpdater
{
    public delegate bool WorkDelayeNotificationHandler(BlockIfDelayedBackgroundUpdater updater, int timesWaited, TimeSpan elapsedWaiting);
    
    public int UpdateDelayFramesBeforeBlock { get; set; }
    public int FramesOfDelay { get; private set; }
    
    /// <summary>
    /// A callback to invoke in the event that the task is delayed. Should return <see langword="false"/> if the updater should stop waiting. Nothing but the task completing will relaunch the task, however.
    /// </summary>
    public WorkDelayeNotificationHandler? WorkDelayed { get; set; }

    // We already know it's completed before we wait
    [SuppressMessage("Usage", "VSTHRD002:Avoid problematic synchronous waits")]
    protected override bool CheckTaskAndAwaitIfReady(Task task, double delta)
    {
        if (task.IsCompleted)
        {
            task.ConfigureAwait(false).GetAwaiter().GetResult();
            return true;
        }
        else if (FramesOfDelay++ >= UpdateDelayFramesBeforeBlock)
        {
            int timesWaited = 0;
            using (StopwatchPool.Shared.ObtainSafe(out var sw))
            {
                sw.Restart();
                while (task.IsCompleted is false)
                {
                    if (WorkDelayed?.Invoke(this, timesWaited++, sw.Elapsed) is false)
                        return false;
                        
                    if (sw.Elapsed > TimeSpan.FromMilliseconds(1))
                        Thread.Sleep(1);
                }
            }
            
            task.ConfigureAwait(false).GetAwaiter().GetResult();
            return true;
        }
        
        return false;
    }
}