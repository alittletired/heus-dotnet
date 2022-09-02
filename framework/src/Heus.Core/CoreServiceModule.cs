using Heus.Ddd.Data;
using Heus.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core;

public class CoreServiceModule:IServiceModule
{
    public void Configure(ConfigureContext context)
    {
     
    }

    public  void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddScoped(typeof(IRepository<>),typeof(DefaultRepository<>));
    }
}