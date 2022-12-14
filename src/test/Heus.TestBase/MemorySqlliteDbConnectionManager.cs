using System.Data.Common;
using Heus.Core.DependencyInjection;
using Heus.Data;
using Heus.Data.Internal;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.TestBase;

[Dependency( ReplaceServices =true, Lifetime= ServiceLifetime.Singleton)]
internal class MemorySqlliteDbConnectionManager : IDbConnectionManager
{
    //内存数据库在单测时只能使用相同的连接
    private readonly DbConnection _shareConnection;
    public MemorySqlliteDbConnectionManager()
    {
        _shareConnection = new SqliteConnection("Filename=:memory:");
        _shareConnection.Open();
    }
    public void Dispose()
    {
        _shareConnection.Dispose();
    }

    public (DbConnection, DbProvider) GetDbConnection<TContext>() where TContext : DbContext
    {
        return (_shareConnection, DbProvider.Sqlite);
    }
}
