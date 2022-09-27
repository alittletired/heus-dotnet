using Heus.Core.DependencyInjection;
using Heus.Data.EfCore.Internal;
using Heus.Ddd;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.EfCore;
[DependsOn(typeof(DddServiceModule))]
public class EfCoreServiceModule:ServiceModuleBase,IPreConfigureServices
{
    private readonly DbContextServiceRegistrar dbContextServiceRegistrar = new DbContextServiceRegistrar();
    public void PreConfigureServices(ServiceConfigurationContext context)
    {
       
        context.AddServiceRegistrar(dbContextServiceRegistrar);
    }
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
       
    }
}