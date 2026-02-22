using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using DiegoG.Godot.Common;
using DiegoG.Godot.Common.Serialization.MessagePack;
using MessagePack;
using MessagePack.Formatters;

namespace DiegoG.Godot.Common;

/// <summary>
/// Provides a means to check on game events as well as additional info about them 
/// </summary>
/// <remarks>
/// This class is meant to keep track of global, permanent events on the game's story that need more information than simply the fact that it happened.
/// </remarks>
[MessagePackFormatter(typeof(EventsTableMessagePackFormatter<,>))]
public class StoryEventTable<TEvent, TContext> 
    where TEvent : unmanaged, Enum
    where TContext : notnull
{
    protected internal readonly ConcurrentDictionary<TEvent, TContext> dict = [];

    public StoryEventTable() : this(new ConcurrentDictionary<TEvent, TContext>()) { }

    internal StoryEventTable(ConcurrentDictionary<TEvent, TContext> dictionary)
    {
        ArgumentNullException.ThrowIfNull(dictionary);
    }
    
    public bool HasEventHappened(TEvent @event, [NotNullWhen(true)] out TContext? info)
        => dict.TryGetValue(@event, out info);

    public bool HasEventHappened(TEvent @event)
        => dict.ContainsKey(@event);

    public void FlagOrOverwriteEvent(TEvent @event, TContext info)
        => dict[@event] = info;

    public bool TryFlagEvent(TEvent @event, TContext info)
        => dict.TryAdd(@event, info);

    /// <summary>
    /// Ensures the event has not been flagged before
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the event has already been flagged</exception>
    public void FlagNewEvent(TEvent @event, TContext info)
    {
        var r = TryFlagEvent(@event, info);
        if (r) throw new InvalidOperationException($"The event '{@event}' has already been risen before");
    }
}
