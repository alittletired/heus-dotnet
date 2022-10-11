﻿using Heus.Core;
using Heus.Core.Data;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Heus.Data.EfCore.Internal
{
    internal class DbContextOptionsFactory : ISingletonDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IConnectionStringResolver  _connectionStringResolver;
        private readonly ILogger<DbContextOptionsFactory> _logger;
        private readonly IOptions<DbContextConfigurationOptions> _options;

        public DbContextOptionsFactory(IUnitOfWorkManager unitOfWorkManager
            , IConnectionStringResolver connectionStringResolver
            , ILogger<DbContextOptionsFactory> logger
            , IOptions<DbContextConfigurationOptions> options)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _connectionStringResolver = connectionStringResolver;
            _logger = logger;
            _options= options;
        }

        public DbContextOptions<TDbContext> Create<TDbContext>() where TDbContext : DbContext
        {
            var unitOfWork = _unitOfWorkManager.Current;
            if (unitOfWork == null)
            {
                throw new BusinessException("A DbContextOptions can only be created inside a unit of work!");
            }
            var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();
            var connectionString = _connectionStringResolver.Resolve(connectionStringName);
            var dbProvider = _options.Value.DefaultDbProvider;
            var dbContextOptionsProvider = _options.Value.DbContextOptionsProviders.First(p=>p.DbProvider== dbProvider);
            var dbConnection= unitOfWork.DbConnections.GetOrAdd(connectionString, connStr =>
            {
                var connection = dbContextOptionsProvider.CreateDbConnection(connStr);
                return connection;
            });
            var builder = new DbContextOptionsBuilder<TDbContext>();
            _logger.LogDebug($" connectionString:{dbConnection.ConnectionString},DbContext:{typeof(TDbContext).Name}");
            _options.Value.ConfigureActions.ForEach(action => action(builder));
            dbContextOptionsProvider.Configure(builder,dbConnection);
            return builder.Options;
        }
    

        

    }
}