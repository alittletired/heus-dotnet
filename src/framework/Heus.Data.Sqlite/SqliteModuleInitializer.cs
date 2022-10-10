using Heus.Core.DependencyInjection;
using Heus.Data.EfCore;

namespace Heus.Data.Sqlite;
[DependsOn(typeof(EfCoreModuleInitializer))]
public class SqliteModuleInitializer : ModuleInitializerBase
{
    
}