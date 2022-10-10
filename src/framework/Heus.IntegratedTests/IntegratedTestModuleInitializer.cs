using Heus.AspNetCore;
using Heus.Core.DependencyInjection;
using Heus.Data.Sqlite;

namespace Heus.IntegratedTests;
[DependsOn(typeof(AspNetModuleInitializer),typeof(SqliteModuleInitializer))]
public class IntegratedTestModuleInitializer : ModuleInitializerBase
{
    
}