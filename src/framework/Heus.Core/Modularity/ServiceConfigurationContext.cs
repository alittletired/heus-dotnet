using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Heus.Core.Modularity;

/// <summary>
/// 对服务容器，配置，环境进行包装，方便服务注册
/// </summary>
public class ServiceConfigurationContext
{
    public  WebApplicationBuilder WebApplicationBuilder { get; }
    public ServiceConfigurationContext(WebApplicationBuilder webApplicationBuilder)
    {
        WebApplicationBuilder = webApplicationBuilder;
        
    }
    public IHostBuilder HostBuilder => WebApplicationBuilder.Host;
    public IServiceCollection Services => WebApplicationBuilder.Services;
    public IConfiguration Configuration => WebApplicationBuilder.Configuration;
    public IHostEnvironment Environment => WebApplicationBuilder.Environment;

}