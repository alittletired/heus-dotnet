using Heus.Core.DependencyInjection;
using Heus.Data.EfCore.Internal;
using Heus.Data.EfCore.Repositories;
using Heus.Ddd;
using Heus.Ddd.Repositories;
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
        context.Services.AddScoped(typeof(IRepository<>),typeof(EfCoreRepository<>));
    }
}