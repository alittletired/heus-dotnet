using Heus.Core.Ioc;
using Heus.Ddd.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core;

public class CoreServiceModule:ServiceModuleBase
{
   
    public override  void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddScoped(typeof(IRepository<>),typeof(DefaultRepository<>));
    }
}