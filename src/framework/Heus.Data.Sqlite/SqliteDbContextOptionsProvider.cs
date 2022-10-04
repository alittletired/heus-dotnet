using System.Data.Common;
using Heus.Core.Data.Options;
using Heus.Data.EfCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.Sqlite;

internal class SqliteDbContextOptionsProvider : IDbContextOptionsProvider
{
    public void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "blogging.db");
        dbContextOptions.UseSqlite($"Data Source={dbPath}");
    }

    public DbProvider DbProvider { get; } = DbProvider.Sqlite;
    public DbConnection CreateDbConnection(string connectionString)
    {
        return new SqliteConnection(connectionString);
    }

}