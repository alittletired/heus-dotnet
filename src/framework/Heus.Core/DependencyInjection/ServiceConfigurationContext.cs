using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.Core.DependencyInjection;

/// <summary>
/// 对服务容器，配置，环境进行包装，方便服务注册
/// </summary>
public class ServiceConfigurationContext
{
    private readonly HostBuilderContext _hostBuilderContext;
    public IServiceCollection Services { get; private set; }

    public ServiceConfigurationContext(HostBuilderContext hostBuilderContext, IServiceCollection services)
    {
        _hostBuilderContext = hostBuilderContext;
        Services = services;

    }
    public IConfiguration Configuration => _hostBuilderContext.Configuration;
    public IHostEnvironment Environment => _hostBuilderContext.HostingEnvironment;

}