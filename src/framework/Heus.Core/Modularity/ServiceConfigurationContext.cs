using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.Core.Modularity;

/// <summary>
/// 对服务容器，配置，环境进行包装，方便服务注册
/// </summary>
public class ServiceConfigurationContext
{
    public ServiceConfigurationContext(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        Services = services;
        Configuration = configuration;
        Environment = environment;
    }

    public IServiceCollection Services { get; }
    public IConfiguration Configuration { get; }
    public IHostEnvironment Environment { get; }

}