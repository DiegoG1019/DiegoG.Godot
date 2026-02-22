using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using GLV.Shared.Common;
using MessagePack;
using MessagePack.Formatters;

namespace DiegoG.Godot.Common.Serialization.MessagePack;

public sealed class EventsTableCollectionMessagePackFormatter<TContext> : IMessagePackFormatter<StoryEventTableCollection<TContext>?>
    where TContext : struct
{
    public void Serialize(ref MessagePackWriter writer, StoryEventTableCollection<TContext>? value, MessagePackSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNil();
            return;
        }
        
        Debug.Assert(value.eventsTableDictionary is not null);
        
        var d = value.eventsTableDictionary;
        writer.WriteMapHeader(d.Count);
        foreach (var (k, v) in d)
        {
            var str = k.AssemblyQualifiedName ?? k.Name;
            var x = Encoding.UTF8.GetByteCount(str);
            using (ArrayPoolHelper.Rent<byte>(x, out var span))
            {
                var r = Encoding.UTF8.TryGetBytes(str, span, out var written);
                Debug.Assert(r);
                writer.WriteString(span[..written]);
            }

            MessagePackSerializer.Serialize(v.GetType(), ref writer, v, options);
        }
    }

    [SuppressMessage("Trimming", "IL2057:Unrecognized value passed to the parameter of method. It\'s not possible to guarantee the availability of the target type.")]
    public StoryEventTableCollection<TContext>? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        if (reader.IsNil) return null;

        var maps = reader.ReadMapHeader();
        Type[] typeParamArray = [ null!, typeof(TContext) ];
        Dictionary<Type, object> dict = new Dictionary<Type, object>(maps);
        for (int i = 0; i < maps; i++)
        {
            var str = reader.ReadString();
            if (str is null) throw new InvalidDataException($"Unable to read type name string value from MessagePack reader, @ {reader.Position}");
            
            var type = Type.GetType(str);

            typeParamArray[0] = type ?? throw new InvalidDataException($"Unable to find type '{str}'");

            var tableType = typeof(StoryEventTable<,>).MakeGenericType(typeParamArray);
            var tab = MessagePackSerializer.Deserialize(tableType, ref reader, options);
            if (tab is null) continue;
            dict.Add(type, tab);
        }
        
        return new StoryEventTableCollection<TContext>(dict);
    }
}