using GLV.Shared.Common;
using Godot;

namespace DiegoG.Godot.Common;

public static class AsyncResourceLoader
{
    private static readonly Cache<string, SemaphoreSlim> SemaphoreCache;

    static AsyncResourceLoader()
    {
        SemaphoreCache = new Cache<string, SemaphoreSlim>();
        SemaphoreCache.RegisterCleanUpAsRecurrentTask();
    }
    
    public static void PreloadInBackground(string resourcePath, string? typeHint = null)
    {
        if (ResourceLoader.HasCached(resourcePath)) return;
        BackgroundTaskStore.Add(LoadCoreAsync(resourcePath, typeHint));
    }

    public static async ValueTask<Resource> LoadAsync(string resourcePath, string? typeHint = null)
    {
        if (ResourceLoader.HasCached(resourcePath)) 
            return ResourceLoader.GetCachedRef(resourcePath);

        return await LoadCoreAsync(resourcePath, typeHint);
    }

    private static async Task<Resource> LoadCoreAsync(string resourcePath, string? typeHint)
    {
        await Task.Yield();
        var sem = (await SemaphoreCache.GetOrAddItemAsync(resourcePath, CreateSemaphore))!;
        await sem.WaitAsync();
        try
        {
            if (ResourceLoader.HasCached(resourcePath)) return ResourceLoader.GetCachedRef(resourcePath);
            // We check this AFTER we lock
            
            var error = ResourceLoader.LoadThreadedRequest(resourcePath, typeHint, useSubThreads: true);
            if (error != Error.Ok)
                throw new FileLoadException($"Could not request to load the resource '{resourcePath}': {error}");
        
            while (true)
            {
                var status = ResourceLoader.LoadThreadedGetStatus(resourcePath);
                
                switch (status)
                {
                    case ResourceLoader.ThreadLoadStatus.Failed or ResourceLoader.ThreadLoadStatus.InvalidResource:
                        throw new FileLoadException($"Could not load resource '{resourcePath}': {status}");
                    case ResourceLoader.ThreadLoadStatus.Loaded:
                        return ResourceLoader.LoadThreadedGet(resourcePath);
                    default:
                        await Task.Delay(250);
                        break;
                }
            }
        }
        finally
        {
            sem.Release();
        }
    }

    private static SemaphoreSlim CreateSemaphore(string arg) 
        => new(1, 1);
}