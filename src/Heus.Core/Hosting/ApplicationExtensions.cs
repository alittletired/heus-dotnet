using Heus.Ioc;
using Heus.Ioc.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.Hosting;

public static class ApplicationExtensions
{
    public static void AddApplication(this IServiceCollection services
        , Type startModuleType
        , IHostEnvironment environment,IConfiguration configuration)
    {
        var application = new HeusApplication(startModuleType);
      
        application.ConfigureServices(services,environment,configuration);
    }
    
    public static void UseApplication(this IHost host)
    {
        var application = host.Services.GetRequiredService<HeusApplication>();
        application.Configure(host);
        // var moduleLoader = new ServiceModuleLoader();
        // var modules = moduleLoader.LoadModules(services, startupModuleType);
        // ConfigureServices(services, modules);
    }
}