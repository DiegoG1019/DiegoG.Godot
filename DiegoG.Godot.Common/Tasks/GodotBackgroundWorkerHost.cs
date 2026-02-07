using GLV.Shared.Common;
using Microsoft.Extensions.DependencyInjection;

namespace DiegoG.Godot.Common.Tasks;

public static class GodotBackgroundWorkerHost
{
    private static bool launched;
    
    public static void LaunchBackgroundWorkers(IServiceProvider services, CancellationTokenSource? cts)
    {
        if (Interlocked.Exchange(ref launched, true))
            throw new InvalidOperationException("Cannot launch Godot Background Workers more than once");
            
        var token = cts?.Token ?? default;
        foreach (var worker in services.GetServices<IGodotBackgroundWorker>())
            BackgroundTaskStore.Add(worker.RunAsync(token));
    }
}