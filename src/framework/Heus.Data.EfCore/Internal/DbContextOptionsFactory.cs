using Heus.Core;
using Heus.Core.Data;
using Heus.Core.Data.Options;
using Heus.Core.DependencyInjection;
using Heus.Core.Uow;
using Heus.Ddd;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace Heus.Data.EfCore.Internal;
internal class DbContextOptionsFactory : ISingletonDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IConnectionStringResolver  _connectionStringResolver;
    private readonly ILogger<DbContextOptionsFactory> _logger;
    private readonly IOptions<RepositoryRegistrationOptions> _options2;
    private readonly DbContextConfigurationOptions _options;
    private readonly IEnumerable<IDbConnectionProvider> _dbConnectionProviders;

    public DbContextOptionsFactory(IUnitOfWorkManager unitOfWorkManager
        , IConnectionStringResolver connectionStringResolver
        , ILogger<DbContextOptionsFactory> logger
        ,IEnumerable<IDbConnectionProvider> dbConnectionProviders
        , IOptions<DbContextConfigurationOptions> options,
        IOptions<RepositoryRegistrationOptions> options2)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _connectionStringResolver = connectionStringResolver;
        _dbConnectionProviders = dbConnectionProviders;
        _logger = logger;
        _options= options.Value;
        _options2 = options2;
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
        var dbProvider = _options.DefaultDbProvider ?? DbProvider.MySql;
        var dbContextOptionsProvider = _dbConnectionProviders.First(p => p.DbProvider == dbProvider);
        var dbConnection = unitOfWork.DbConnections.GetOrAdd(connectionString, dbContextOptionsProvider.CreateConnection);
        var builder = new DbContextOptionsBuilder<TDbContext>();
        builder.LogTo(Console.WriteLine, LogLevel.Debug).EnableSensitiveDataLogging().EnableDetailedErrors();
        _logger.LogDebug(" connectionString:{ConnectionString},DbContext:{DbContext}",connectionString,typeof(TDbContext).Name);
        _options.DbContextOptionsActions.ForEach(action => action(builder));
        dbContextOptionsProvider.Configure(builder,dbConnection);
      
        return builder.Options;
    }

}
