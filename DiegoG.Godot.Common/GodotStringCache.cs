using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Godot;

namespace DiegoG.Godot.Common;

public readonly struct GodotStringCache
{
    private static readonly ConcurrentDictionary<string, GodotStringCache> StringCache = new(); 
    
    private GodotStringCache(string str, StringName strName)
    {
        StringName = strName;
        DotNetString = str;
    }
    public GodotStringCache(string str) : this(str, str) { }
    public GodotStringCache(StringName stringName) : this(stringName, stringName) { }
    
    public StringName StringName { get; }
    public string DotNetString { get; }

    public static GodotStringCache FromCache([CallerMemberName] string str = null!)
    {
        ArgumentNullException.ThrowIfNull(str);
        return StringCache.GetOrAdd(str, (s) => new GodotStringCache(s));
    }

    public static implicit operator StringName(GodotStringCache ch) => ch.StringName;
}