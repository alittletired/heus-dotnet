using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.DependencyInjection;
/// <summary>
/// 服务模块
/// </summary>
public interface IModuleInitializer
{
    void PreConfigureServices(ServiceConfigurationContext context);
    void PostConfigureServices(ServiceConfigurationContext context);
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    void ConfigureServices(ServiceConfigurationContext context);

    /// <summary>
    /// 配置应用
    /// </summary>
    /// <param name="context"></param>
   Task Configure(ApplicationConfigurationContext context);


}