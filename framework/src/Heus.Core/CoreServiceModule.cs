using Heus.Ddd.Data;
using Heus.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core;

public class CoreServiceModule:ServiceModuleBase
{
    public override void ConfigureServices(ConfigureServicesContext context)
    {
        context.Services.AddScoped(typeof(IRepository<>),typeof(DefaultRepository<>));
        base.ConfigureServices(context);
    }
}