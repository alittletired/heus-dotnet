using Heus.Core.DependencyInjection;
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.EfCore.Internal;

internal class DefaultDbContextProvider : IDbContextProvider,IScopedDependency
{
    private readonly IDbContextServiceRegistrar _dbContextServiceRegistrar;
    private readonly IServiceProvider _serviceProvider;
    public DefaultDbContextProvider(IDbContextServiceRegistrar dbContextServiceRegistrar
        , IServiceProvider serviceProvider)
    {
        _dbContextServiceRegistrar = dbContextServiceRegistrar;
        _serviceProvider = serviceProvider;
    }
    public  DbContext GetDbContext<TEntity>() where TEntity : class, IEntity
    {
        var dbContextType = _dbContextServiceRegistrar.EntityDbContextMapping[typeof(TEntity)];
        //var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(dbContextType);
        //var connectionString =await _connectionStringResolver.ResolveAsync(connectionStringName);
        return (DbContext)_serviceProvider.GetRequiredService(dbContextType);
    }
}
