namespace Heus.Core.DependencyInjection;
public abstract class ModuleInitializerBase : IModuleInitializer
{
    public virtual void PreConfigureServices(ServiceConfigurationContext context)
    {

    }
    public virtual void ConfigureServices(ServiceConfigurationContext context)
    {

    }

    public virtual void PostConfigureServices(ServiceConfigurationContext context)
    {
        
    }

    public virtual void Configure(IApplicationBuilder app)
    {
        
    }
}