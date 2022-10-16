namespace Heus.Core.DependencyInjection;

internal interface IServiceRegistrar { 

   
    void OnRegistered(Action<Type> registrationAction);
    void AddMiddlewares(IServiceRegistrarMiddleware middleware);
    void OnScan(Action<Type> scanAction);
    void Registrar(IServiceCollection services, Type type);

}