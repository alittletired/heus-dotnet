using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Ioc;

public class ServiceModuleDescriptor
{
    public Type Type { get; }

    public Assembly Assembly { get; }

    public IServiceModule Instance { get; }


    public IReadOnlyList<ServiceModuleDescriptor> Dependencies => _dependencies.ToImmutableList();
    private readonly List<ServiceModuleDescriptor> _dependencies;

    public ServiceModuleDescriptor(
         Type type,
         IServiceModule instance
        )
    {
        if (!type.GetTypeInfo().IsAssignableFrom(instance.GetType()))
        {
            throw new ArgumentException($"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}");
        }

        Type = type;
        Assembly = type.Assembly;
        Instance = instance;
        _dependencies = new List<ServiceModuleDescriptor>();
    }

    public void AddDependency(ServiceModuleDescriptor descriptor)
    {
        _dependencies.TryAdd(descriptor);
    }


}

