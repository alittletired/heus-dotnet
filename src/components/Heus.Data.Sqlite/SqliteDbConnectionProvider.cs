using System.Data.Common;
using Heus.Core.DependencyInjection;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.Sqlite;

public class SqliteDbConnectionProvider : IDbConnectionProvider, ISingletonDependency
{
   
    public void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection)
    {

        dbContextOptions.UseSqlite(shareConnection);
    }

    public DbProvider DbProvider { get; } = DbProvider.Sqlite;

    public DbProviderFactory DbProviderFactory => SqliteFactory.Instance;

    public DbConnection CreateConnection(string connectionString)
    {
        return new SqliteConnection(connectionString);
    }


}