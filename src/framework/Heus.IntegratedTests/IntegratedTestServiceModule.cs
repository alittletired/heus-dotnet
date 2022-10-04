using Heus.AspNetCore;
using Heus.Core.DependencyInjection;
using Heus.Data.Sqlite;

namespace Heus.IntegratedTests;
[DependsOn(typeof(AspNetServiceModule),typeof(SqliteServiceModule))]
public class IntegratedTestServiceModule:ServiceModuleBase
{
    
}