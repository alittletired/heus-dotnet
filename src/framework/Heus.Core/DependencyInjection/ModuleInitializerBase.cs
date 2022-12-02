namespace Heus.Core.DependencyInjection;
public abstract class ModuleInitializerBase : IModuleInitializer
{
    public virtual string? Name { get; } = null;
   public virtual void PreConfigureServices(ServiceConfigurationContext context)
    {

    }
    public virtual void ConfigureServices(ServiceConfigurationContext context)
    {

    }

    public virtual void PostConfigureServices(ServiceConfigurationContext context)
    {
        
    }


    public virtual Task InitializeAsync(IServiceProvider serviceProvider)
    {
        return Task.CompletedTask;
    }
}