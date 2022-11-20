using Heus.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Data.Internal;
using Microsoft.Extensions.DependencyInjection;
using Heus.Data.Utils;

namespace Heus.Data;
[DependsOn(typeof(CoreModuleInitializer))]
public class DataModuleInitializer : ModuleInitializerBase
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<DbConnectionOptions>(context.Configuration);
        
    }
  

}