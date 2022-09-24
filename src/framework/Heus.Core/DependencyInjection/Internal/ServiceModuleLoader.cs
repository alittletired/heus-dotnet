using Heus.Core.DependencyInjection;

namespace Heus.Core.DependencyInjection.Internal;

internal sealed class ServiceModuleLoader
{
    public List<ServiceModuleDescriptor> LoadModules(Type startupModuleType)
    {
        var modules = GetDescriptors(startupModuleType);
        modules = SortByDependency(modules, startupModuleType);
        return modules.ToList();
    }

    private List<ServiceModuleDescriptor> GetDescriptors(Type startupModuleType)
    {
        var modules = new List<ServiceModuleDescriptor>();
        FillModules(modules, startupModuleType);
        SetDependencies(modules);

        return modules.ToList();
    }

    private void FillModules(List<ServiceModuleDescriptor> modules, Type startupModuleType)
    {
        //All modules starting from the startup module
        foreach (var moduleType in ServiceModuleHelper.FindAllModuleTypes(startupModuleType))
        {
            modules.Add(CreateModuleDescriptor(moduleType));
        }


    }

    private void SetDependencies(List<ServiceModuleDescriptor> modules)
    {
        foreach (var module in modules)
        {
            SetDependencies(modules, module);
        }
    }

    private List<ServiceModuleDescriptor> SortByDependency(List<ServiceModuleDescriptor> modules,
        Type startupModuleType)
    {
        var sortedModules = modules.SortByDependencies(m => m.Dependencies);
        sortedModules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
        return sortedModules.Distinct().ToList();
    }

    private ServiceModuleDescriptor CreateModuleDescriptor(
        Type moduleType)
    {
        return new ServiceModuleDescriptor(moduleType, CreateAndRegisterModule(moduleType));
    }

    private IServiceModule CreateAndRegisterModule(Type moduleType)
    {
        var module = (IServiceModule)Activator.CreateInstance(moduleType)!;
        // services.AddSingleton(moduleType, module);
        return module;
    }

    private void SetDependencies(List<ServiceModuleDescriptor> modules, ServiceModuleDescriptor module)
    {
        foreach (var dependedModuleType in ServiceModuleHelper.FindDependedModuleTypes(module.Type))
        {
            var dependedModule = modules.FirstOrDefault(m => m.Type == dependedModuleType);
            if (dependedModule == null)
            {
                throw new Exception("Could not find a depended module " +
                                    dependedModuleType.AssemblyQualifiedName + " for " +
                                    module.Type.AssemblyQualifiedName);
            }

            module.AddDependency(dependedModule);
        }
    }
}

