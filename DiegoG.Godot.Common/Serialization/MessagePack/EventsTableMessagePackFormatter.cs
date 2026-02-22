using System.Collections.Concurrent;
using MessagePack;
using MessagePack.Formatters;

namespace DiegoG.Godot.Common.Serialization.MessagePack;

public class EventsTableMessagePackFormatter<TEvent, TContext> : IMessagePackFormatter<StoryEventTable<TEvent, TContext>?>
    where TEvent : unmanaged, Enum
    where TContext : struct 
{
    public void Serialize(ref MessagePackWriter writer, StoryEventTable<TEvent, TContext>? value, MessagePackSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNil();
            return;
        }

        MessagePackSerializer.Serialize(ref writer, value.dict, options);
    }

    public StoryEventTable<TEvent, TContext>? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (reader.IsNil) return null;
        var dict = MessagePackSerializer.Deserialize<ConcurrentDictionary<TEvent, TContext>>(
            ref reader,
            options
        );

        return new StoryEventTable<TEvent, TContext>(dict);
    }
}