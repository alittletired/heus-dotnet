using Heus.Core;
using Heus.Core.Data;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Uow;
using Microsoft.EntityFrameworkCore;


namespace Heus.Data.EfCore.Internal
{
    internal class DbContextOptionsFactory : ISingletonDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IEnumerable<IDbContextOptionsProvider> _dbContextOptionsProviders;
        IConnectionStringResolver _connectionStringResolver;


        public DbContextOptionsFactory(IUnitOfWorkManager unitOfWorkManager
            , IEnumerable<IDbContextOptionsProvider> dbContextOptionsProviders,
IConnectionStringResolver connectionStringResolver)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _dbContextOptionsProviders = dbContextOptionsProviders;
            _connectionStringResolver = connectionStringResolver;
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
            //todo:目前 没有处理嵌套事务,和多数据库驱动场景，后面来补
            var dbContextOptionsProvider = _dbContextOptionsProviders.First();
          
            var dbConnection= unitOfWork.DbConnections.GetOrAdd(connectionString, connStr =>
            {
                var connection = dbContextOptionsProvider.CreateDbConnection(connStr);
                return connection;
            });
            var builder = new DbContextOptionsBuilder<TDbContext>();
            dbContextOptionsProvider.Configure(builder,dbConnection);
            return builder.Options;
        }
    

        

    }
}
