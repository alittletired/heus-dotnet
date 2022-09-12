using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.Ioc;

public class ServiceModuleBase:IServiceModule
{
    public virtual void ConfigureServices(ServiceConfigurationContext context)
    {
    
    }

    public virtual void ConfigureApplication(ApplicationConfigurationContext context)
    {
       
    }
}