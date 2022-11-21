using System.Collections.Concurrent;
using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using MySqlConnector;


namespace Heus.Data.Mysql;

internal class MySqlDbConnectionProvider : IDbConnectionProvider
{
    private static readonly ConcurrentDictionary<string, ServerVersion> _serverVersions = new();
    public DbProviderFactory DbProviderFactory => MySqlConnectorFactory.Instance;
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

   

}
