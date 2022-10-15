namespace Heus.Core.DependencyInjection;

public interface IServiceRegistrar { 

    /// <summary>
    /// ע�����ʱ�ص�
    /// </summary>
    /// <param name="registrationAction"></param>
    void OnRegistred(Action<Type> registrationAction);
    /// <summary>
    /// ɨ�����г��򼯵�����ʱ�ص�
    /// </summary>
    /// <param name="scanAction"></param>
    void OnScan(Action<Type> scanAction);

}