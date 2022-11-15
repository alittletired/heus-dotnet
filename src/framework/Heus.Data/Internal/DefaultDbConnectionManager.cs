using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Heus.Core.DependencyInjection;
using Heus.Core.Uow;

namespace Heus.Data.Internal;
internal class DefaultDbConnectionManager : IDbConnectionManager, IScopedDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IConnectionStringResolver _connectionStringResolver;
    private readonly IEnumerable<IDbConnectionProvider> _dbConnectionProviders;
    private readonly DbContextConfigurationOptions _options;

    private readonly Dictionary<string, DbConnection> _connections = new();
    public DefaultDbConnectionManager(IUnitOfWorkManager unitOfWorkManager, IConnectionStringResolver connectionStringResolver
        , IEnumerable<IDbConnectionProvider> dbConnectionProviders, DbContextConfigurationOptions options)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _connectionStringResolver = connectionStringResolver;
        _dbConnectionProviders = dbConnectionProviders;
        _options = options;
    }

    public DbConnection GetDbConnection<TDbContext>() where TDbContext : DbContext
    {
        var unitOfWork = _unitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            throw new BusinessException("A DbContextOptions can only be created inside a unit of work!");
        }
        var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();

        var connectionString = _connectionStringResolver.Resolve(connectionStringName);
        var dbContextOptionsProvider = _dbConnectionProviders.First(p => p.DbProvider == _options.DbProvider);
        var dbConnection = unitOfWork.DbConnections.GetOrAdd(connectionString, dbContextOptionsProvider.CreateConnection);
    }
}
