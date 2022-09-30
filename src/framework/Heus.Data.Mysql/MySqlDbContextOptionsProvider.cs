﻿using System.Collections.Concurrent;
using System.Data.Common;
using Heus.Core.Data.Options;
using Heus.Data.EfCore;
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



        public string ProviderName => throw new NotImplementedException();
    }
}
