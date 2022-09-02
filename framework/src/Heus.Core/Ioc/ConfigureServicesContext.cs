using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.Ioc;
public class ConfigureServicesContext
{
    public IServiceCollection Services { get; }
    public IHostEnvironment Environment => Services.GetSingletonInstance<IHostEnvironment>()!;
    public IConfiguration Configuration => Services.GetSingletonInstance<IConfiguration>()!;
    public ConfigureServicesContext(IServiceCollection services)
    {
        Services = services;
     
    }
}

public class ConfigureContext
{
    public IServiceProvider ServiceProvider { get; }
    public IHostEnvironment Environment => ServiceProvider.GetRequiredService<IHostEnvironment>();

    public ConfigureContext(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

}