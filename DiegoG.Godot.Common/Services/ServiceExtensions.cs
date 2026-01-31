using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using GLV.Shared.Common;
using GLV.Shared.DependencyInjection;
using Godot;
using Microsoft.Extensions.DependencyInjection;

namespace DiegoG.Godot.Common.Services;

public static class ServiceExtensions
{
    private sealed class ServiceSceneLoadClosure<T>(string scenePath) where T : Node
    {
        private PackedScene? scene;
        public T Get(IServiceProvider _) => (scene ??= GD.Load<PackedScene>(scenePath)).Instantiate<T>();
        public T GetKeyed(IServiceProvider _, object? k) => (scene ??= GD.Load<PackedScene>(scenePath)).Instantiate<T>();
    }
    
    private sealed record ServiceSceneClosureWithKey(Func<object?, Node> Factory)
    {
        public object FactoryWrapper(IServiceProvider s, object? k) => Factory.Invoke(k);
    }
    
    private sealed record ServiceSceneClosureWithProviderAndNoKey(Func<IServiceProvider, Node> Factory)
    {
        public object FactoryWrapper(IServiceProvider s, object? k) => Factory.Invoke(s);
    }
    
    private sealed record ServiceSceneClosure(Func<Node> Factory)
    {
        public object FactoryWrapper(IServiceProvider _) => Factory.Invoke();
        public object FactoryWrapper(IServiceProvider s, object? k) => Factory.Invoke();
    }
    
    public static IServiceCollection AddNode<T>(this IServiceCollection services, Func<IServiceProvider, T>? factory = null) where T : Node 
        => factory is null ? services.AddTransient<T>() : services.AddTransient<T>(factory);
    
    public static IServiceCollection AddKeyedNode<T>(this IServiceCollection services, object? key, Func<IServiceProvider, object?, T>? factory = null) where T : Node 
        => factory is null ? services.AddKeyedTransient<T>(key) : services.AddKeyedTransient<T>(key, factory);

    public static IServiceCollection AddScene<T>(this IServiceCollection services, string scenePath) where T : Node
        => services.AddTransient<T>(new ServiceSceneLoadClosure<T>(scenePath).Get);

    public static IServiceCollection AddKeyedScene<T>(this IServiceCollection services, object? key, string scenePath)
        where T : Node
        => services.AddKeyedTransient<T>(key, new ServiceSceneLoadClosure<T>(scenePath).GetKeyed);

    public static void RegisterDecoratedServiceNodesAndScenes(
        this IServiceCollection serviceCollection, 
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] params HashSet<Assembly>? assemblies
    )
    {
        foreach (var (type, attributes) in AssemblyInfo.GetTypesWithAttributesFromAssemblies<RegisterServiceNode>(assemblies))
        {
            if (type.IsAssignableTo(typeof(Node)) is false)
                throw new InvalidDataException("The 'RegisterServiceNodeAttribute' is only valid on Node types");
            
            foreach (var attr in attributes)
            {
                var factory = type.GetMethod("Instantiate", BindingFlags.Static);
                if (factory is not null &&
                    factory.ReturnParameter.ParameterType.IsAssignableTo(type) &&
                    factory.ContainsGenericParameters is false)
                {
                    var checkResult = ValidateParams(factory.GetParameters());
                    if (checkResult is >= 0)
                    {
                        ServiceDescriptor desc = checkResult switch
                        {
                            // When 0, it receives no params.
                            // When 1, it receives one param that is an IServiceProvider
                            // When 2, it receives two params, the first one is an IServiceProvider, and the second a nullable object
                            
                            0 when string.IsNullOrWhiteSpace(attr.ServiceKey) => new ServiceDescriptor(
                                type,
                                new ServiceSceneClosure(factory.CreateDelegate<Func<Node>>()).FactoryWrapper,
                                ServiceLifetime.Transient
                            ),
                            
                            0 when string.IsNullOrWhiteSpace(attr.ServiceKey) is false => new ServiceDescriptor(
                                type,
                                attr.ServiceKey,
                                new ServiceSceneClosure(factory.CreateDelegate<Func<Node>>()).FactoryWrapper,
                                ServiceLifetime.Transient
                            ),
                            
                            1 when string.IsNullOrWhiteSpace(attr.ServiceKey) => new ServiceDescriptor(
                                type,
                                factory.CreateDelegate<Func<IServiceProvider, Node>>(), 
                                ServiceLifetime.Transient
                            ),
                            
                            1 when string.IsNullOrWhiteSpace(attr.ServiceKey) is false => new ServiceDescriptor(
                                type,
                                attr.ServiceKey,
                                new ServiceSceneClosureWithProviderAndNoKey(factory.CreateDelegate<Func<IServiceProvider, Node>>()).FactoryWrapper,
                                ServiceLifetime.Transient
                            ),
                            
                            2 => new ServiceDescriptor(
                                type,
                                attr.ServiceKey,
                                factory.CreateDelegate<Func<IServiceProvider, object?, Node>>(),
                                ServiceLifetime.Transient
                            ),
                            
                            _ => throw new ApplicationException("This code should not be executed")
                        };

                        serviceCollection.Add(desc);
                    }

                    continue;
                }
                
                serviceCollection.Add(string.IsNullOrWhiteSpace(attr.ServiceKey) is false
                    ? new ServiceDescriptor(type, attr.ServiceKey, type, ServiceLifetime.Transient)
                    : new ServiceDescriptor(type, type, ServiceLifetime.Transient));
            }
            continue;

            static int ValidateParams(ParameterInfo[] p) 
                => p.Length switch
                {
                    0 => 0,
                    1 when typeof(IServiceProvider).IsAssignableTo(p[0].ParameterType) => 1,
                    2 when typeof(IServiceProvider).IsAssignableTo(p[0].ParameterType) && p[1].ParameterType == typeof(object) => 2,
                    _ => -1
                };
        }

        MethodInfo? addScnMethod = typeof(ServiceExtensions).GetMethod(nameof(AddScene));
        Debug.Assert(addScnMethod is not null);
        Debug.Assert(addScnMethod.IsGenericMethodDefinition);
        
        MethodInfo? addKeyedScnMethod = typeof(ServiceExtensions).GetMethod(nameof(AddKeyedScene));
        Debug.Assert(addKeyedScnMethod is not null);
        Debug.Assert(addKeyedScnMethod.IsGenericMethodDefinition);

        Type[] genParams = new Type[1];
        object?[] addScnParams = new object?[2] { serviceCollection, null };
        object?[] addKeyedScnParams = new object?[3] { serviceCollection, null, null };
        
        foreach (var (type, attributes) in AssemblyInfo.GetTypesWithAttributesFromAssemblies<RegisterServiceScene>(assemblies))
        {
            if (type.IsAssignableTo(typeof(Node)) is false)
                throw new InvalidDataException("The 'RegisterServiceSceneAttribute' is only valid on Node types");
                
            foreach (var attr in attributes)
            {
                if (string.IsNullOrWhiteSpace(attr.ServiceKey) is false)
                {
                    genParams[0] = type;
                    var m = addScnMethod.MakeGenericMethod(genParams);

                    addScnParams[1] = attr.ScenePath;
                    m.Invoke(null, addScnParams);
                }
                else
                {
                    genParams[0] = type;
                    var m = addKeyedScnMethod.MakeGenericMethod(genParams);

                    addKeyedScnParams[1] = attr.ServiceKey;
                    addKeyedScnParams[2] = attr.ScenePath;
                    m.Invoke(null, addKeyedScnParams);
                }
            }
        }
    }
}