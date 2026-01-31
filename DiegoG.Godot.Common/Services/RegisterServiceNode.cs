namespace DiegoG.Godot.Common.Services;

[AttributeUsage(AttributeTargets.Class)]
public sealed class RegisterServiceNode : Attribute
{
    public string? ServiceKey { get; init; }
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class RegisterServiceScene : Attribute
{
    public RegisterServiceScene(string scenePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(scenePath);
        ScenePath = scenePath;
    }

    public string ScenePath { get; }
    public string? ServiceKey { get; init; }
}