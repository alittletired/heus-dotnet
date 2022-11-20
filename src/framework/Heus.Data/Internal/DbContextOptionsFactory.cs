
using Microsoft.Extensions.Options;
namespace Heus.Data.Internal;
internal class DbContextOptionsFactory<TContext> : IDbContextOptionsFactory<TContext> where TContext : DbContext {

   
    private readonly DataOptions _options;
    private readonly IDbConnectionManager _dbConnectionManager;
    private readonly IUowDbCommandInterceptor _commandInterceptor;
    private readonly IUowDbConnectionInterceptor _uowDbConnectionInterceptor;
    public DbContextOptionsFactory(IOptions<DataOptions> options
        , IDbConnectionManager dbConnectionManager
        , IUowDbConnectionInterceptor uowDbConnectionInterceptor,
        IUowDbCommandInterceptor commandInterceptor)
    {
        _options = options.Value;
        _dbConnectionManager = dbConnectionManager;
        _uowDbConnectionInterceptor = uowDbConnectionInterceptor;
        _commandInterceptor = commandInterceptor;
    }

    public DbContextOptions<TContext> CreateOptions()
    {
        var dbConnection = _dbConnectionManager.GetDbConnection<TContext>();
        var builder = new DbContextOptionsBuilder<TContext>();
        _options.ConfigureDbContextOptions.ForEach(configure => configure(builder));
        //todo: 目前没有想好如何填充中间件实例，故先使用构造函数传入
        //builder.AddInterceptors(_options.Interceptors);
        builder.AddInterceptors(_uowDbConnectionInterceptor,_commandInterceptor);
        var dbContextOptionsProvider = _options.DbConnectionProviders.First(p => p.DbProvider == _options.DbProvider);
        dbContextOptionsProvider.Configure(builder, dbConnection);
        return builder.Options;
    }

}
