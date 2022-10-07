using Heus.Core;
using Heus.Core.Data;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Heus.Data.EfCore.Internal
{
    internal class DbContextOptionsFactory : ISingletonDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IEnumerable<IDbContextOptionsProvider> _dbContextOptionsProviders;
        private readonly IConnectionStringResolver  _connectionStringResolver;
        private readonly ILogger<DbContextOptionsFactory> _logger;

        public DbContextOptionsFactory(IUnitOfWorkManager unitOfWorkManager
            , IEnumerable<IDbContextOptionsProvider> dbContextOptionsProviders
            , IConnectionStringResolver connectionStringResolver
            , ILogger<DbContextOptionsFactory> logger)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _dbContextOptionsProviders = dbContextOptionsProviders;
            _connectionStringResolver = connectionStringResolver;
            _logger = logger;
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
            //todo:目前 没有处理嵌套事务,和多数据库，多db的切换驱动场景，后面来补
            var dbContextOptionsProvider = _dbContextOptionsProviders.First();
          
            var dbConnection= unitOfWork.DbConnections.GetOrAdd(connectionString, connStr =>
            {
                var connection = dbContextOptionsProvider.CreateDbConnection(connStr);
                return connection;
            });
            var builder = new DbContextOptionsBuilder<TDbContext>();
            _logger.LogDebug($" connectionString:{dbConnection.ConnectionString},DbContext:{typeof(TDbContext).Name}");
            dbContextOptionsProvider.Configure(builder,dbConnection);
            return builder.Options;
        }
    

        

    }
}
