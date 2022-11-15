using System.Collections.Concurrent;
using System.Data.Common;
using Heus.Data;
using Heus.Data.Options;
using Heus.Core.DependencyInjection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.Sqlite;

internal class SqliteDbConnectionProvider : IDbConnectionProvider, ISingletonDependency
{
    private readonly ConcurrentDictionary<string, DbConnection> _connections = new();
    public void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection)
    {

        dbContextOptions.UseSqlite(shareConnection);
    }

    public DbProvider DbProvider { get; } = DbProvider.Sqlite;
    public DbConnection CreateConnection(string connectionString)
    {
        return _connections.GetOrAdd(connectionString,
            connStr => new SqliteConnection(connectionString));
    }

    public void Dispose()
    {
        foreach (var connection in _connections.Values)
        {
            connection.Dispose();
        }
        _connections.Clear();
    }
}