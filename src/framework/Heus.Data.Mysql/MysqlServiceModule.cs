using Heus.Core.DependencyInjection;
using Heus.Data.EfCore;
using Heus.Ddd;

namespace Heus.Data.Mysql;

[DependsOn(typeof(DddServiceModule), typeof(EfCoreServiceModule))]
public class MysqlServiceModule:ServiceModuleBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
    }
}