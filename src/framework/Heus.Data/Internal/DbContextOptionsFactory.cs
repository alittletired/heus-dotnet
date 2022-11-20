using System.Reflection;
using Heus.Core.DependencyInjection;
using Microsoft.Extensions.Options;
namespace Heus.Data.Internal;
internal class DbContextOptionsFactory : IDbContextOptionsFactory,IScopedDependency {

    private static MethodInfo _createOptions = typeof(DbContextOptionsFactory).GetTypeInfo()
        .DeclaredMethods.First(s => s.Name == nameof(CreateOptions) && s.IsGenericMethod);
    private readonly DataOptions _options;
    private readonly IDbConnectionManager _dbConnectionManager;

    private readonly IUowDbConnectionInterceptor _uowDbConnectionInterceptor;
    public DbContextOptionsFactory(IOptions<DataOptions> options
        , IDbConnectionManager dbConnectionManager
        , IUowDbConnectionInterceptor uowDbConnectionInterceptor)
    {
        _options = options.Value;
        _dbConnectionManager = dbConnectionManager;
        _uowDbConnectionInterceptor = uowDbConnectionInterceptor;
    }
    public DbContextOptions CreateOptions(Type contextType)
    {
        return (DbContextOptions)_createOptions.MakeGenericMethod(contextType).Invoke(this,null)!;
    }
    public DbContextOptions<TContext> CreateOptions<TContext>() where TContext : DbContext
    {
        var dbConnection = _dbConnectionManager.GetDbConnection<TContext>();
        var builder = new DbContextOptionsBuilder<TContext>();
        _options.ConfigureDbContextOptions.ForEach(configure => configure(builder));
        //todo: 目前没有想好如何填充中间件实例，故先使用构造函数传入
        //builder.AddInterceptors(_options.Interceptors);
        builder.AddInterceptors(_uowDbConnectionInterceptor);
        var dbContextOptionsProvider = _options.DbConnectionProviders.First(p => p.DbProvider == _options.DbProvider);
        dbContextOptionsProvider.Configure(builder, dbConnection);
        return builder.Options;
    }

}
