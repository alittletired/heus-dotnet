using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.DependencyInjection;
/// <summary>
/// 服务模块
/// </summary>
public interface IModuleInitializer
{
    /// <summary>
    /// 模块名，用于设置区域
    /// </summary>
    string? Name { get; }
    void PreConfigureServices(ServiceConfigurationContext context);
    void PostConfigureServices(ServiceConfigurationContext context);
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    void ConfigureServices(ServiceConfigurationContext context);

    Task InitializeAsync(IServiceProvider serviceProvider);


}