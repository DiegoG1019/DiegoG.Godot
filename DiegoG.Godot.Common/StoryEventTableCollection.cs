using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using DiegoG.Godot.Common;
using DiegoG.Godot.Common.Serialization.MessagePack;
using GLV.Shared.Common;
using MessagePack;
using MessagePack.Formatters;

namespace DiegoG.Godot.Common;

public sealed class StoryEventTableCollection<TContext> where TContext : notnull
{
    private readonly Lock @lock = new();
    internal readonly Dictionary<Type, object> eventsTableDictionary;

    public StoryEventTableCollection() : this(new Dictionary<Type, object>()) { }

    internal StoryEventTableCollection(Dictionary<Type, object> dict)
    {
        ArgumentNullException.ThrowIfNull(dict);
        eventsTableDictionary = dict;
    }

    public StoryEventTable<TEvent, TContext> GetTable<TEvent>() where TEvent : unmanaged, Enum
    {
        lock (@lock)
        {
            ref var tab = ref CollectionsMarshal.GetValueRefOrAddDefault(eventsTableDictionary, typeof(TEvent), out _);
            return (StoryEventTable<TEvent, TContext>)(tab ??= new StoryEventTable<TEvent, TContext>());
        }
    }
}