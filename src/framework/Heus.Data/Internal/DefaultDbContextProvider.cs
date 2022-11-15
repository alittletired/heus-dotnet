using Heus.Core.DependencyInjection;
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heus.Data.EfCore.Internal;

internal class DefaultDbContextProvider : IDbContextProvider,IScopedDependency
{
    private readonly IOptions<DbContextConfigurationOptions> _options;
    private readonly IServiceProvider _serviceProvider;
    public DefaultDbContextProvider( IOptions<DbContextConfigurationOptions> options
        , IServiceProvider serviceProvider)
    {
        _options = options;
        _serviceProvider = serviceProvider;
    }
    public  DbContext GetDbContext<TEntity>() where TEntity : class, IEntity
    {
        var dbContextType = _options.Value.EntityDbContextMappings[typeof(TEntity)];
        //var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(dbContextType);
        //var connectionString =await _connectionStringResolver.ResolveAsync(connectionStringName);
        return (DbContext)_serviceProvider.GetRequiredService(dbContextType);
    }
}
