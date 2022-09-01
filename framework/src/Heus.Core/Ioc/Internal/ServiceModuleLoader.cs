
namespace Heus.Ioc.Internal;

internal class ServiceModuleLoader
{
    public List<ServiceModuleDescriptor> LoadModules( Type startupModuleType)
    {

        var modules = GetDescriptors( startupModuleType);
        modules = SortByDependency(modules, startupModuleType);
        return modules.ToList();
    }

    private List<ServiceModuleDescriptor> GetDescriptors( Type startupModuleType)
    {
        var modules = new List<ServiceModuleDescriptor>();

        FillModules(modules,  startupModuleType);
        SetDependencies(modules);

        return modules.ToList();
    }

    protected virtual void FillModules(
        List<ServiceModuleDescriptor> modules,
       
        Type startupModuleType)
    {

        //All modules starting from the startup module
        foreach (var moduleType in ServiceModuleHelper.FindAllModuleTypes(startupModuleType))
        {
            modules.Add(CreateModuleDescriptor( moduleType));
        }


    }

    protected virtual void SetDependencies(List<ServiceModuleDescriptor> modules)
    {
        foreach (var module in modules)
        {
            SetDependencies(modules, module);
        }
    }

    protected virtual List<ServiceModuleDescriptor> SortByDependency(List<ServiceModuleDescriptor> modules,
        Type startupModuleType)
    {
        var sortedModules = modules.SortByDependencies(m => m.Dependencies);
        sortedModules.MoveItem(m => m.Type == startupModuleType, modules.Count - 1);
        return sortedModules.Distinct().ToList();
    }

    protected virtual ServiceModuleDescriptor CreateModuleDescriptor(
     Type moduleType)
    {
        return new ServiceModuleDescriptor(moduleType, CreateAndRegisterModule( moduleType));
    }

    protected virtual IServiceModule CreateAndRegisterModule( Type moduleType)
    {
        var module = (IServiceModule)Activator.CreateInstance(moduleType)!;
        // services.AddSingleton(moduleType, module);
        return module;
    }

    protected virtual void SetDependencies(List<ServiceModuleDescriptor> modules, ServiceModuleDescriptor module)
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

