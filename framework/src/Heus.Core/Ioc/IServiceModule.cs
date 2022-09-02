using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.Ioc;
/// <summary>
/// 服务模块
/// </summary>
public interface IServiceModule
{

    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="services"></param>
    void ConfigureServices(IServiceCollection services);

    /// <summary>
    /// 配置应用
    /// </summary>
    /// <param name="serviceProvider"></param>
    void ConfigureApplication(IServiceProvider serviceProvider);


}