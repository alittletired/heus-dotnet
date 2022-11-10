using System.Collections.Concurrent;
using System.Data.Common;
using Heus.Core.Data;
using Heus.Core.Data.Options;
using Heus.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;


namespace Heus.Data.Mysql;

internal class MySqlDbConnectionProvider : IDbConnectionProvider, IScopedDependency
{
    private static readonly ConcurrentDictionary<string, ServerVersion> _serverVersions = new();

    public void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection)
    {

        var serverVersion = _serverVersions.GetOrAdd(shareConnection.ConnectionString,
            (key) => ServerVersion.AutoDetect(shareConnection as MySqlConnection));
        dbContextOptions.UseMySql(shareConnection, serverVersion,
            mySqlOptions =>
            {
                mySqlOptions.EnableStringComparisonTranslations();
                mySqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
    }

    public DbProvider DbProvider => DbProvider.MySql;

    public DbConnection CreateConnection(string connectionString)
    {
        return new MySqlConnection(connectionString);
    }

    public void Dispose()
    {

    }


}
