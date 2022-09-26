using Heus.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;
using System.Threading;

namespace Heus.Core.DependencyInjection.Internal;
public class DefaultServiceRegistrar : IServiceRegistrar
{
    public virtual void Handle(IServiceCollection services, Type type,ServiceRegistrarChain chain)
    {
        if (!TryGetServiceLifetime(type,out var lifeTime)) {
            return;
        }
        foreach (var serviceType in GetServiceTypes(type))
        {
            var descriptor = ServiceDescriptor.Describe(serviceType, type, lifeTime!);
            services.Add(descriptor);
        }
    }
    protected  bool TryGetServiceLifetime(Type type,out ServiceLifetime lifeTime)
    {

        lifeTime = default;
        if (type.IsAssignableTo<ISingletonDependency>())
        {
            lifeTime = ServiceLifetime.Singleton;
            return true;
        }
        if (type.IsAssignableTo<IScopedDependency>())
        {
            lifeTime= ServiceLifetime.Scoped;
            return true;
        }
        return false;
    }


    protected List<Type> GetServiceTypes(Type type)
    {
        var serviceTypes = new List<Type>();

        serviceTypes.AddRange(GetDefaultServices(type));

        return serviceTypes;
    }

    protected List<Type> GetDefaultServices(Type type)
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