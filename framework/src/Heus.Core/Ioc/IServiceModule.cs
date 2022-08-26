namespace Heus.Ioc;
/// <summary>
/// 服务模块
/// </summary>
public interface IServiceModule
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    void ConfigureServices(ConfigureServicesContext context);

    /// <summary>
    /// 配置应用
    /// </summary>
    /// <param name="context"></param>
    void Configure(ConfigureContext context);


}