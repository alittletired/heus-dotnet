using Heus.Core.DependencyInjection;
using Heus.Data.EfCore;

namespace Heus.Data.Sqlite;
[DependsOn(typeof(EfCoreServiceModule))]
public class SqliteServiceModule:ServiceModuleBase
{
    
}