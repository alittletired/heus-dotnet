namespace Heus.Core.DependencyInjection;

public interface IServiceRegistrar { 

    /// <summary>
    /// 注册服务时回调
    /// </summary>
    /// <param name="registrationAction"></param>
    void OnRegistred(Action<Type> registrationAction);
    /// <summary>
    /// 扫描所有程序集的类型时回调
    /// </summary>
    /// <param name="scanAction"></param>
    void OnScan(Action<Type> scanAction);

}