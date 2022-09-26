using Heus.Core.DependencyInjection;
using Heus.Ddd;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.EfCore;
[DependsOn(typeof(DddServiceModule))]
public class EfCoreServiceModule:ServiceModuleBase,IPreConfigureServices
{
    public void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.ServiceRegistrars.Add(new DbContextServiceRegistrar());
    }
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton(new DbContextOptionsManager());
    }
}