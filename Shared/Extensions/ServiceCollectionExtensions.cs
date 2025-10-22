using MillionPropertyApi.Shared.Interfaces;

namespace MillionPropertyApi.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddModule<T>(this IServiceCollection services) 
        where T : class, IModule
    {
        var module = Activator.CreateInstance<T>();
        module.RegisterServices(services);
        return services;
    }
}
