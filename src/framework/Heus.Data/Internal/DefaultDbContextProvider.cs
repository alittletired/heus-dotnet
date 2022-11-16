using Heus.Core.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heus.Data.Internal;

internal class DefaultDbContextProvider : IDbContextProvider,IScopedDependency
{
    private readonly IOptions<DataOptions> _options;
    private readonly IServiceProvider _serviceProvider;
    public DefaultDbContextProvider( IOptions<DataOptions> options
        , IServiceProvider serviceProvider)
    {
        _options = options;
        _serviceProvider = serviceProvider;
    }
    public  DbContext GetDbContext<TEntity>() 
    {
        var dbContextType = _options.Value.EntityDbContextMappings[typeof(TEntity)];
        //var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(dbContextType);
        //var connectionString =await _connectionStringResolver.ResolveAsync(connectionStringName);
        return (DbContext)_serviceProvider.GetRequiredService(dbContextType);
    }
}
