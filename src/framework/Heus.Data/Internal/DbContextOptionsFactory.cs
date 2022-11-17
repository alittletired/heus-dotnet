using Heus.Core.DependencyInjection;
using Heus.Core.Uow;
using Heus.Data.Internal;

using Microsoft.Extensions.Options;
namespace Heus.Data.EfCore.Internal;
internal class DbContextOptionsFactory : IDbContextOptionsFactory, ISingletonDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    private readonly IDbConnectionManager _dbConnectionManager;
    private readonly DataOptions _options;
    public DbContextOptionsFactory(IUnitOfWorkManager unitOfWorkManager
        , IDbConnectionManager dbConnectionManager
        , IOptions<DataOptions> options)
    {
        _unitOfWorkManager = unitOfWorkManager;
        _dbConnectionManager = dbConnectionManager;

        _options = options.Value;
    }

    public DbContextOptions<TDbContext> Create<TDbContext>() where TDbContext : DbContext
    {
        var unitOfWork = _unitOfWorkManager.Current;
        var dbConnection = _dbConnectionManager.GetDbConnection<TDbContext>();
        var builder = new DbContextOptionsBuilder<TDbContext>();
        _options.ConfigureDbContextOptions.ForEach(configure => configure(builder));
        var dbContextOptionsProvider = _options.DbConnectionProviders.First(p => p.DbProvider == _options.DbProvider);
        dbContextOptionsProvider.Configure(builder, dbConnection);
        return builder.Options;
    }

}
