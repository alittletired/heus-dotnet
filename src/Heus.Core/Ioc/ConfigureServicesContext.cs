using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.Ioc;
public class ConfigureServicesContext
{
    public IServiceCollection Services { get; }
    public IConfiguration Configuration { get; }
    public IHostEnvironment Environment { get; }
    public ConfigureServicesContext(IServiceCollection services
        , IHostEnvironment hostEnvironment
        ,IConfiguration configuration)
    {
        Services = services;
        Environment = hostEnvironment;
        Configuration = configuration;
    }
}

public class ConfigureContext
{
    public IHost Host { get; }
    public IServiceProvider ServiceProvider => Host.Services;
    public IHostEnvironment Environment => ServiceProvider.GetRequiredService<IHostEnvironment>();

    public ConfigureContext(IHost host)
    {
        Host = host;
    }

}