using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Heus.Data.Postgres;

internal class PostgresSqlDbConnectionProvider : IDbConnectionProvider
{


    public void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection)
    {
        dbContextOptions.UseNpgsql(shareConnection,
           options =>
            {
                //options.EnableStringComparisonTranslations();
                options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
    }

    public DbProvider DbProvider => DbProvider.Postgres;

    public DbProviderFactory DbProviderFactory => NpgsqlFactory.Instance;

    public DbConnection CreateConnection(string connectionString)
    {
        return new NpgsqlConnection(connectionString);
    }

 


}
