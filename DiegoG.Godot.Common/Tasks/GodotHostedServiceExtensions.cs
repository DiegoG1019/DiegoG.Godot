using System.Reflection;
using GLV.Shared.Common;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DiegoG.Godot.Common.Tasks;

public static class GodotHostedServiceExtensions
{
    public static void RegisterDecoratedGodotWorkers(
        this IServiceCollection services,
        params HashSet<Assembly>? assemblies
    )
    {
        foreach (var (type, _) in AssemblyInfo.GetTypesWithAttributeFromAssemblies<RegisterGodotWorkerAttribute>(assemblies))
        {
            var desc = new ServiceDescriptor(typeof(IGodotBackgroundWorker), type, ServiceLifetime.Singleton);
            services.TryAddEnumerable(desc);
        }
    }
}