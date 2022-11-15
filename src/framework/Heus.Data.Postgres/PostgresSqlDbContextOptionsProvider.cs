using System.Data.Common;
using Heus.Data;

using Heus.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Heus.Data.Options;

namespace Heus.Data.Postgres;

internal class PostgresSqlDbConnectionProvider : IDbConnectionProvider, IScopedDependency
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

    public DbProvider DbProvider => DbProvider.PostgreSql;

    public DbConnection CreateConnection(string connectionString)
    {
        return new NpgsqlConnection(connectionString);
    }

    public void Dispose()
    {

    }


}
