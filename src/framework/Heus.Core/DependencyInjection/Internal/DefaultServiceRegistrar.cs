using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace Heus.Core.DependencyInjection.Internal;
internal class DefaultServiceRegistrar : IServiceRegistrar { 

   
    public event EventHandler<Type>? ServiceRegistered;
    public event EventHandler<Type>? TypeScaning;
    public event EventHandler<Assembly>? ModuleInitialized;

    public void RegistrarModule(IServiceCollection services, Assembly assembly)
    {
        ModuleInitialized?.Invoke(services, assembly);
        var types = assembly.GetTypes()
              .Where(type => type.IsClass &&
                             !type.IsAbstract &&
                             !type.IsGenericType).ToList();
        foreach (var type in types)
        {

            Registrar(services, type);

        }
    }
    
    private void Registrar(IServiceCollection services, Type type)
    {
        TypeScaning?.Invoke(services, type);
        var dependencyAttribute = type.GetCustomAttribute<DependencyAttribute>(true);
        var lifeTime = dependencyAttribute?.Lifetime ?? GetServiceLifetime(type);
        if (lifeTime == null)
        {
            return;
        }
        
        foreach (var serviceType in GetServiceTypes(type))
        {
            var descriptor = ServiceDescriptor.Describe(serviceType, type, lifeTime.Value);
            if (dependencyAttribute?.ReplaceServices == true)
            {
                services.Replace(descriptor);
            }
            else
            {
                services.Add(descriptor);
            }

        }

        // _middlewares.ForEach(m => m.OnRegister(context));
        ServiceRegistered?.Invoke(services, type);
    }

    protected ServiceLifetime? GetServiceLifetime(Type type)
    {
        if (type.IsAssignableTo<ISingletonDependency>())
        {
            return  ServiceLifetime.Singleton;
          
        }
        if (type.IsAssignableTo<IScopedDependency>())
        {
            return ServiceLifetime.Scoped;
           
        }
        return default;
    }

    public static List<Type> GetServiceTypes(Type type)
    {
        var serviceTypes = GetInterfaceServices(type);
        serviceTypes.Add(type);
        return serviceTypes;
    }

    public  static  List<Type> GetInterfaceServices(Type type)
    {
        var serviceTypes = new List<Type>();

        foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
        {
            var interfaceName = interfaceType.Name!;

            if (interfaceName.StartsWith("I"))
            {
                interfaceName = interfaceName[1..];
            }

            if (type.Name.EndsWith(interfaceName))
            {
                serviceTypes.Add(interfaceType);
            }

        }

        return serviceTypes;
    }

  
}