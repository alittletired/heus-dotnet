using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.Sqlite;

public class SqliteDbConnectionProvider : IDbConnectionProvider
{
   
    public void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection)
    {

        dbContextOptions.UseSqlite(shareConnection);
    }

    public DbProvider DbProvider  => DbProvider.Sqlite;

    public DbProviderFactory DbProviderFactory => SqliteFactory.Instance;

    public virtual DbConnection CreateConnection(string connectionString)
    {
        return new SqliteConnection(connectionString);
    }


}