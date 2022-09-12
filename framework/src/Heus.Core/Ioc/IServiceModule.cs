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
    /// <param name="context"></param>
    void ConfigureServices(ServiceConfigurationContext context);

    /// <summary>
    /// 配置应用
    /// </summary>
    /// <param name="context"></param>
    void ConfigureApplication(ApplicationConfigurationContext context);


}