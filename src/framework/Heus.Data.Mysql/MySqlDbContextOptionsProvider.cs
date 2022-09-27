using System.Collections.Concurrent;
using System.Data.Common;
using Heus.Data.EfCore;
using Heus.Ddd.Data.Options;
using Microsoft.EntityFrameworkCore;


namespace Heus.Data.Mysql
{
    internal class MySqlDbContextOptionsProvider : IDbContextOptionsProvider
    {
        private ConcurrentDictionary<string, ServerVersion> _serverVersions = new();

        public void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection)
        {

            var serverVersion = _serverVersions.GetOrAdd(shareConnection.ConnectionString, ServerVersion.AutoDetect);
            dbContextOptions.UseMySql(shareConnection, serverVersion,
                mySqlOptions => { mySqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); });
        }

        public DbProvider DbProvider => DbProvider.MySql;
    }
}
