using Heus.Core.DependencyInjection;

using Heus.Ddd;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.Mysql;

[DependsOn(typeof(DataModuleInitializer))]
public class MysqlModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<DataOptions>(options =>
        {
            options.DbConnectionProviders.Add(new MySqlDbConnectionProvider());
        });
    }
}