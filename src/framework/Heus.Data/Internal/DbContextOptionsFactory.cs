
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
namespace Heus.Data.Internal;
internal class DbContextOptionsFactory<TContext> : IDbContextOptionsFactory<TContext> where TContext : DbContext {

   
    private readonly DataOptions _options;
    private readonly IDbConnectionManager _dbConnectionManager;
    private readonly IEnumerable<IInterceptor>  _dbInterceptor;
    public DbContextOptionsFactory(IOptions<DataOptions> options
        , IDbConnectionManager dbConnectionManager
        , IEnumerable<IInterceptor> dbInterceptor)
    {
        _options = options.Value;
        _dbConnectionManager = dbConnectionManager;
        _dbInterceptor = dbInterceptor;
    }

    public DbContextOptions<TContext> CreateOptions()
    {
        var connectionWrapper = _dbConnectionManager.GetDbConnection<TContext>();
        var builder = new DbContextOptionsBuilder<TContext>();
        _options.ConfigureDbContextOptions.ForEach(configure => configure(builder));
        //todo: 目前没有想好如何填充中间件实例，故先使用构造函数传入
        //builder.AddInterceptors(_options.Interceptors);
        builder.AddInterceptors(_dbInterceptor);
        connectionWrapper.DbConnectionProvider.Configure(builder, connectionWrapper.DbConnection);
        return builder.Options;
    }

}
