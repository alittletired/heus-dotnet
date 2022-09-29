using Heus.Core.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Core.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core;

public class CoreServiceModule:ServiceModuleBase
{
   
    public override  void ConfigureServices(ServiceConfigurationContext context)
    {
      
        context.Services.ConfigureOptions<DbConnectionOptions>();
    }
}