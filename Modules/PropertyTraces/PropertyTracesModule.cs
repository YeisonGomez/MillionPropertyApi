using Microsoft.Extensions.DependencyInjection;
using MillionPropertyApi.Modules.PropertyTraces.Interfaces;
using MillionPropertyApi.Modules.PropertyTraces.Services;
using MillionPropertyApi.Shared.Interfaces;

namespace MillionPropertyApi.Modules.PropertyTraces;

public class PropertyTracesModule : IModule
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IPropertyTraceService, PropertyTraceService>();
    }
}
