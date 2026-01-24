namespace DiegoG.Godot.Common;

public interface IAsyncUpdateable
{
    public Task ProcessAsync(double delta);
}