using System.Collections.Concurrent;
using System.Data.Common;
using Heus.Core.Data;
using Heus.Core.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Data.EfCore;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;


namespace Heus.Data.Mysql
{
    internal class MySqlDbContextOptionsProvider : IDbConnectionProvider,IScopedDependency
    {
        private static readonly ConcurrentDictionary<string, ServerVersion> _serverVersions = new();
        private readonly ConcurrentDictionary<string, DbConnection> _connections = new();

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
        public DbConnection CreateConnection(string connectionString)
        {
           return  _connections.GetOrAdd(connectionString, connStr => new MySqlConnection(connectionString));
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
}
