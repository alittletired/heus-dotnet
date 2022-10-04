using System.Reflection;
using Heus.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.ObjectMapping;

public class ObjectMapperRegistrar:IServiceRegistrar
{
    public void Handle(IServiceCollection services, Type type, ServiceRegistrarChain chain)
    {
        var attributes = type.GetTypeInfo().GetCustomAttributes<ObjectMapperAttribute>();
        if (attributes == null)
        {
            chain.Next(services,type);
            return;
        }

        foreach (var attr in attributes)
        {
            services.AddObjectMap(attr.MappingType,type);   
        }
    }
}