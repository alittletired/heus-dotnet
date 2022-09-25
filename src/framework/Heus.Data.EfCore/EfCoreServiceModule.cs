using Heus.Core.DependencyInjection;
using Heus.Ddd;

namespace Heus.Data.EfCore;
[DependsOn(typeof(DddServiceModule))]
public class EfCoreServiceModule:ServiceModuleBase,IPreConfigureServices
{
    public void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.ServiceRegistrars.Add(new DbContextServiceRegistrar());
    }
}