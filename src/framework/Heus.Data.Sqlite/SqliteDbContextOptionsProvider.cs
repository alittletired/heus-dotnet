using System.Data.Common;
using Heus.Core.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Data.EfCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.Sqlite;

internal class SqliteDbContextOptionsProvider : IDbContextOptionsProvider
{
    public void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection)
    {
        var folder = AppContext.BaseDirectory;
        var dbPath = Path.Join(folder, "heus.db");
        dbContextOptions.UseSqlite($"Data Source={dbPath}");
    }

    public DbProvider DbProvider { get; } = DbProvider.Sqlite;
    public DbConnection CreateDbConnection(string connectionString)
    {
        return new SqliteConnection(connectionString);
    }

}