

namespace Heus.Core.DependencyInjection.Internal;

internal sealed class ServiceModuleLoader
{
    public List<ServiceModuleDescriptor> LoadModules(Type startupModuleType, List<Type> additionalModules)
    {
        var moduleTypes = new HashSet<Type>();
        
        foreach (var moduleType in ServiceModuleHelper.FindAllModuleTypes(startupModuleType))
        {
           
            moduleTypes.Add(moduleType);
        }

        foreach (var moduleType in additionalModules)
        {
            foreach (var type in  ServiceModuleHelper.FindAllModuleTypes(moduleType))
            {
                moduleTypes.Add(type);
            }
        
        }

        var modules =moduleTypes.Select(CreateModuleDescriptor).ToList();
        SetDependencies(modules);
        Dictionary<ServiceModuleDescriptor, int> orders = new();
        foreach (var module in modules)
        {
            CalcDependenciesCount(module, orders);
        }
      
        modules = modules.OrderBy(s =>orders[s] ).ToList();
        return modules;
    }

    private int CalcDependenciesCount(ServiceModuleDescriptor descriptor,
        Dictionary<ServiceModuleDescriptor, int> orders)
    {
        if (orders.TryGetValue(descriptor, out var count))
        {
            return count;
        }

        count = 1 + descriptor.Dependencies.Select(s => CalcDependenciesCount(s, orders)).Sum();
        
        orders[descriptor] = count;
        return count;
    }

    private  void SetDependencies(List<ServiceModuleDescriptor> modules)
    {
        foreach (var module in modules)
        {
            foreach (var dependedModuleType in ServiceModuleHelper.FindDependedModuleTypes(module.Type))
            {
                var dependedModule = modules.FirstOrDefault(m => m.Type == dependedModuleType);
                if (dependedModule == null)
                {
                    throw new Exception("Could not find a depended module " + dependedModuleType.AssemblyQualifiedName + " for " + module.Type.AssemblyQualifiedName);
                }

                module.AddDependency(dependedModule);
            }
        }
    }
    private ServiceModuleDescriptor CreateModuleDescriptor(
        Type moduleType)
    {
        return new ServiceModuleDescriptor(moduleType, CreateAndRegisterModule(moduleType));
    }

    private ModuleInitializerBase CreateAndRegisterModule(Type moduleType)
    {
        var module = (ModuleInitializerBase)Activator.CreateInstance(moduleType)!;
        // services.AddSingleton(moduleType, module);
        return module;
    }

   
}

