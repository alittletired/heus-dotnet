using System.Reflection;

namespace Heus.Core.DependencyInjection;

public interface IServiceRegistrar {


    event EventHandler<Type>? ServiceRegistered;
    event EventHandler<Type>? TypeScaning;
    event EventHandler<Assembly>? ModuleInitialized;
   
    void RegistrarModule(IServiceCollection services, Assembly assembly);

}