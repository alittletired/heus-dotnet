using Heus.Core.DependencyInjection;
using Heus.Core.Uow;
using Heus.Data.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace Heus.Data.EfCore.Internal;
internal class DbContextOptionsFactory : ISingletonDependency,IDisposable
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IDbConnectionManager _dbConnectionManager;
    private readonly ILogger<DbContextOptionsFactory> _logger;
    private readonly DbContextConfigurationOptions _options;
  

    public DbContextOptionsFactory(
        IUnitOfWorkManager unitOfWorkManager
,     IDbConnectionManager dbConnectionManager
        , ILogger<DbContextOptionsFactory> logger
        , IOptions<DbContextConfigurationOptions> options)
    {
        _unitOfWorkManager = unitOfWorkManager;
            _dbConnectionManager = dbConnectionManager;


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

    public void Dispose()
    {
        
    }
}
