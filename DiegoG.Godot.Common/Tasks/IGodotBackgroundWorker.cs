namespace DiegoG.Godot.Common.Tasks;

public interface IGodotBackgroundWorker
{
    Task RunAsync(CancellationToken stoppingToken);
}