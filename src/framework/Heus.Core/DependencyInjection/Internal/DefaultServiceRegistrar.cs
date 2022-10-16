using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace Heus.Core.DependencyInjection.Internal;
internal class DefaultServiceRegistrar : IServiceRegistrar { 

    private readonly List<IServiceRegistrarMiddleware> _middlewares = new();
    private readonly List<Action<Type>> _registrationActions = new();
    private readonly List<Action<Type>> _scanActions = new();
    public void OnRegistered(Action<Type> registrationAction)
    {
        _registrationActions.Add(registrationAction);
    }

    public void AddMiddlewares(IServiceRegistrarMiddleware middleware)
    {
        throw new NotImplementedException();
    }

    public void Registrar(IServiceCollection services, Type type)
    {
        _scanActions.ForEach(s => s(type));

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
            else if (dependencyAttribute?.TryRegister == true)
            {
                services.TryAdd(descriptor);

            }
            else
            {
                services.Add(descriptor);
            }

        }

        // _middlewares.ForEach(m => m.OnRegister(context));
        _registrationActions.ForEach(action => action(type));
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

   

 

    public void OnScan(Action<Type> scanAction)
    {
        throw new NotImplementedException();
    }
}