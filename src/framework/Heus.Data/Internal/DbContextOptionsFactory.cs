using Heus.Data;
using Heus.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Core.Uow;
using Heus.Ddd;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace Heus.Data.EfCore.Internal;
internal class DbContextOptionsFactory : ISingletonDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IConnectionStringResolver _connectionStringResolver;
    private readonly ILogger<DbContextOptionsFactory> _logger;
    private readonly DbContextConfigurationOptions _options;
    private readonly IEnumerable<IDbConnectionProvider> _dbConnectionProviders;

    public DbContextOptionsFactory(
        IUnitOfWorkManager unitOfWorkManager
        , IConnectionStringResolver connectionStringResolver
        , ILogger<DbContextOptionsFactory> logger
        , IEnumerable<IDbConnectionProvider> dbConnectionProviders
        , IOptions<DbContextConfigurationOptions> options)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _connectionStringResolver = connectionStringResolver;
        _dbConnectionProviders = dbConnectionProviders;
        _logger = logger;
        _options = options.Value;
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
        var dbContextOptionsProvider = _dbConnectionProviders.First(p => p.DbProvider == _options.DbProvider);
        var dbConnection = unitOfWork.DbConnections.GetOrAdd(connectionString, dbContextOptionsProvider.CreateConnection);
        var builder = new DbContextOptionsBuilder<TDbContext>();

        _logger.LogDebug(" connectionString:{ConnectionString},DbContext:{DbContext}", connectionString, typeof(TDbContext).Name);
        _options.DbContextOptionsActions.ForEach(action => action(builder));
        dbContextOptionsProvider.Configure(builder, dbConnection);

        return builder.Options;
    }

}
