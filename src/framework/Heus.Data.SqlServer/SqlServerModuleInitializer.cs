﻿
using Heus.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
namespace Heus.Data.SqlServer;

[DependsOn(typeof(DataModuleInitializer))]
public class SqlServerModuleInitializer : ModuleInitializerBase
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<DataOptions>(options =>
        {
            options.DbConnectionProviders.Add(new SqlServerDbConnectionProvider());
        });
    }
}