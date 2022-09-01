using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.Core.Ioc;

public static class ServiceModuleExtensions
{
    public static IServiceCollection AddServiceModule(this IServiceCollection services, Type startModuleType)
    {
        ServiceModuleManager manager = new ServiceModuleManager(startModuleType);
        manager.ConfigureServices(services);
        return services;
    }
    public static void UseServiceModule(IHost host)
    {
        var manager = host.Services.GetRequiredService<ServiceModuleManager>();
        manager.Configure(host);

    }
}
