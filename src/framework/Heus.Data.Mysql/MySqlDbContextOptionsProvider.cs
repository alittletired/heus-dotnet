using System.Collections.Concurrent;
using System.Data.Common;
using Heus.Core.Data.Options;
using Heus.Data.EfCore;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;


namespace Heus.Data.Mysql
{
    internal class MySqlDbContextOptionsProvider : IDbContextOptionsProvider
    {
        private readonly ConcurrentDictionary<string, ServerVersion> _serverVersions = new();

        public void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection)
        {

            var serverVersion = _serverVersions.GetOrAdd(shareConnection.ConnectionString, ServerVersion.AutoDetect);
            dbContextOptions.UseMySql(shareConnection, serverVersion,
                mySqlOptions =>
                {
                    mySqlOptions.EnableStringComparisonTranslations();
                    mySqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
        }

        public DbProvider DbProvider => DbProvider.MySql;
        public DbConnection CreateDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
