using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.Ioc;

public class ServiceModuleBase:IServiceModule
{
   
    public virtual void ConfigureServices(IServiceCollection services)
    {
     
    }

    public virtual  void ConfigureApplication(IServiceProvider serviceProvider)
    {
        throw new NotImplementedException();
    }
}