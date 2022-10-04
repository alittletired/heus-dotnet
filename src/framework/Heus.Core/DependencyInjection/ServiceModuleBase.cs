namespace Heus.Core.DependencyInjection;
public abstract class ServiceModuleBase : IServiceModule
{
    public virtual void ConfigureServices(ServiceConfigurationContext context)
    {

    }

    public virtual Task ConfigureApplication(ApplicationConfigurationContext context)
    {
        return Task.CompletedTask;
    }
    
}