using Microsoft.Extensions.DependencyInjection;

using System.Reflection;
namespace Heus.Ioc.Internal;
public class DefaultServiceRegistrar : IServiceRegistrar
{
    public void Handle(IServiceCollection services, Type type)
    {

        var serviceAttr = type.GetCustomAttribute<ServiceAttribute>();
        if (serviceAttr == null)
            return;
        foreach (var serviceType in GetServiceTypes(type, serviceAttr))
        {
            var descriptor = ServiceDescriptor.Describe(
                serviceType,
                type,
                serviceAttr.LifeTime
            );
            services.Add(descriptor);
        }
    }



    protected List<Type> GetServiceTypes(Type type, ServiceAttribute attribute)
    {
        var serviceTypes = new List<Type>();

        serviceTypes.AddRange(GetDefaultServices(type));

        return serviceTypes;
    }

    private List<Type> GetDefaultServices(Type type)
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