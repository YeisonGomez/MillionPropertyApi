using Microsoft.Extensions.DependencyInjection;
using MillionPropertyApi.Shared.Interfaces;
using MillionPropertyApi.Modules.Properties.Interfaces;
using MillionPropertyApi.Modules.Properties.Services;

namespace MillionPropertyApi.Modules.Properties;

public class PropertiesModule : IModule
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IPropertyImageService, PropertyImageService>();
        services.AddScoped<IPropertyService, PropertyService>();
    }
}
