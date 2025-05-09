using Microsoft.Extensions.DependencyInjection;

namespace Saruman.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAllImplementationsOfInterface<TInterface>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        var interfaceType = typeof(TInterface);
        var implementations = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => interfaceType.IsAssignableFrom(type) && type is { IsClass: true, IsAbstract: false });

        foreach (var impl in implementations)
        {
            services.Add(new ServiceDescriptor(interfaceType, impl, lifetime));
        }
    }
}