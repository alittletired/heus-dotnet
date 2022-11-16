using System.Data.Common;
using Heus.Core.DependencyInjection;
using Heus.Core.Uow;
using Heus.Data.EfCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Heus.Data.Internal;
internal class DefaultDbConnectionManager : IDbConnectionManager, IScopedDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IConnectionStringResolver _connectionStringResolver;
    private readonly DataOptions _options;
    private readonly ILogger<DefaultDbConnectionManager> _logger;

    private readonly Dictionary<string, DbConnection> _connections = new();
    public DefaultDbConnectionManager(IUnitOfWorkManager unitOfWorkManager
        , IConnectionStringResolver connectionStringResolver

        , IOptions<DataOptions> options,
     ILogger<DefaultDbConnectionManager> logger)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _connectionStringResolver = connectionStringResolver;
        _options = options.Value;
        _logger = logger;
    }

    public void Dispose()
    {
        _logger.LogInformation("Dispose connections");
        _connections.Values.ForEach(c => c.Dispose());
        _connections.Clear();
    }

    public DbConnection GetDbConnection<TDbContext>() where TDbContext : DbContext
    {
        var unitOfWork = _unitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            throw new BusinessException("A DbContextOptions can only be created inside a unit of work!");
        }
        unitOfWork.Disposed += (o,e) => Dispose();
         var connectionStringName = ConnectionStringNameAttribute.GetConnStringName<TDbContext>();

        var connectionString = _connectionStringResolver.Resolve(connectionStringName);

        var dbConnection = _connections.GetOrAdd(connectionString, (cs) =>
        {
            var dbContextOptionsProvider = _options.DbConnectionProviders.First(p => p.DbProvider == _options.DbProvider);
            _logger.LogDebug(" connectionString:{ConnectionString},DbContext:{DbContext}", connectionString, typeof(TDbContext).Name);
            return dbContextOptionsProvider.CreateConnection(cs);
        });


        return dbConnection;
    }
}
