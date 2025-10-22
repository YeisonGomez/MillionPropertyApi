using Microsoft.Extensions.DependencyInjection;
using MillionPropertyApi.Modules.Owners.Interfaces;
using MillionPropertyApi.Modules.Owners.Services;
using MillionPropertyApi.Shared.Interfaces;

namespace MillionPropertyApi.Modules.Owners;

public class OwnersModule : IModule
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IOwnerService, OwnerService>();
    }
}
