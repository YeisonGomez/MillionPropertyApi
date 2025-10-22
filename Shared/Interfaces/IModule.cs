using Microsoft.Extensions.DependencyInjection;

namespace MillionPropertyApi.Shared.Interfaces;

public interface IModule
{
    void RegisterServices(IServiceCollection services);
}
