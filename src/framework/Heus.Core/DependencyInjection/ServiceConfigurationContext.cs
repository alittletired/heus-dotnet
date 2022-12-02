
using Microsoft.Extensions.Configuration;

namespace Heus.Core.DependencyInjection;

/// <summary>
/// 对服务容器，配置，环境进行包装，方便服务注册
/// </summary>
public class ServiceConfigurationContext
{
    
    public required  IServiceCollection Services { get; init; }
    public required IConfiguration Configuration { get; init; }
    public required IServiceRegistrar ServiceRegistrar { get;init; }

}