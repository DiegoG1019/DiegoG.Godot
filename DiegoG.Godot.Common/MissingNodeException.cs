namespace DiegoG.Godot.Common;

/// <summary>
/// Exception that is thrown when an attempt to get a required Godot Node fails
/// </summary>
public class MissingNodeException : MissingMemberException
{
    public MissingNodeException()
    {
    }

    public MissingNodeException(string message) : base(message)
    {
    }

    public MissingNodeException(string message, Exception inner) : base(message, inner)
    {
    }
}

public class MissingNodeException<T> : MissingNodeException
{
    public MissingNodeException() : base(ComposeMessage(null))
    {
    }

    public MissingNodeException(string message) : base(ComposeMessage(message))
    {
    }

    public MissingNodeException(string message, Exception inner) : base(ComposeMessage(message), inner)
    {
    }

    private static string ComposeMessage(string? msg)
        => string.IsNullOrWhiteSpace(msg) 
            ? $"Node Type: {typeof(T).AssemblyQualifiedName}" 
            : $"{msg}; Node Type: {typeof(T).AssemblyQualifiedName}";
}