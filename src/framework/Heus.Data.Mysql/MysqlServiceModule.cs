using Heus.Core.DependencyInjection;
using Heus.Ddd;

namespace Heus.Data.Mysql;

[DependsOn(typeof(DddServiceModule))]
public class MysqlServiceModule:ServiceModuleBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
    }
}