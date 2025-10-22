using Microsoft.Extensions.DependencyInjection;
using MillionPropertyApi.Modules.PropertyImages.Interfaces;
using MillionPropertyApi.Modules.PropertyImages.Services;
using MillionPropertyApi.Shared.Interfaces;

namespace MillionPropertyApi.Modules.PropertyImages;

public class PropertyImagesModule : IModule
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IPropertyImageService, PropertyImageService>();
    }
}
