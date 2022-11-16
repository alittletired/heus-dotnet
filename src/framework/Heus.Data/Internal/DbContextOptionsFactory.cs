using Heus.Core.DependencyInjection;
using Heus.Core.Uow;
using Heus.Data.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace Heus.Data.EfCore.Internal;
internal class DbContextOptionsFactory : ISingletonDependency, IDisposable
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IDbConnectionManager _dbConnectionManager;
    private readonly ILogger<DbContextOptionsFactory> _logger;
    private readonly DataConfigurationOptions _options;


    public DbContextOptionsFactory(IUnitOfWorkManager unitOfWorkManager
        , IDbConnectionManager dbConnectionManager
        , ILogger<DbContextOptionsFactory> logger
        , IOptions<DataConfigurationOptions> options)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _dbConnectionManager = dbConnectionManager;
        _logger = logger;
        _options = options.Value;
    }
    private DbContextOptionsBuilder<TDbContext> CreateOptionsBuilder<TDbContext>() where TDbContext : DbContext
    {
        var builder = new DbContextOptionsBuilder<TDbContext>();

        var logLevel = LogLevel.Information;
//#if DEBUG
//        logLevel = LogLevel.Debug;
//#endif
        builder.UseSnakeCaseNamingConvention();
        builder.LogTo(Console.WriteLine, logLevel)
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors();
        return builder;
    }
    public DbContextOptions<TDbContext> Create<TDbContext>() where TDbContext : DbContext
    {
        var unitOfWork = _unitOfWorkManager.Current;

        var dbConnection = _dbConnectionManager.GetDbConnection<TDbContext>();
        var builder = CreateOptionsBuilder<TDbContext>();
        var dbContextOptionsProvider = _options.DbConnectionProviders.First(p => p.DbProvider == _options.DbProvider);
        dbContextOptionsProvider.Configure(builder, dbConnection);
        return builder.Options;
    }

    public void Dispose()
    {

    }
}
