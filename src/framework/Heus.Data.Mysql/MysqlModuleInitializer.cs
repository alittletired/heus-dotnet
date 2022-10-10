using Heus.Core.DependencyInjection;
using Heus.Data.EfCore;
using Heus.Ddd;

namespace Heus.Data.Mysql;

[DependsOn(typeof(EfCoreModuleInitializer))]
public class MysqlModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
    }
}