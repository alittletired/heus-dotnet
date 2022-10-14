using Heus.Core.DependencyInjection.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.Core.DependencyInjection;

/// <summary>
/// 对服务容器，配置，环境进行包装，方便服务注册
/// </summary>
public class ServiceConfigurationContext
{
    internal List<IServiceRegistrar> ServiceRegistrars { get; } = new();

    public void AddServiceRegistrar(IServiceRegistrar serviceRegistrar)
    {
        ServiceRegistrars.Add(serviceRegistrar);
        var serviceTypes = DefaultServiceRegistrar.GetServiceTypes(serviceRegistrar.GetType());
        foreach (var serviceType in serviceTypes)
        {
            Services.AddSingleton(serviceType, serviceRegistrar);
        }

    }

    public IServiceCollection Services { get; }
    public IConfiguration Configuration { get; }
   
    public ServiceConfigurationContext(IServiceCollection services
        , IConfiguration configuration
       )
    {

        Services = services;
        Configuration = configuration;
        AddServiceRegistrar(new DefaultServiceRegistrar());
    }

  

}