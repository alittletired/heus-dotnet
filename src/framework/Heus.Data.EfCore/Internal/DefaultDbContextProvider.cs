using Heus.Core.Data;
using Heus.Ddd.Data;
using Heus.Ddd.Entities;
using Heus.Ddd.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.EfCore.Internal;

internal class DefaultDbContextProvider : IDbContextProvider
{
    private readonly IDbContextServiceRegistrar _dbContextServiceRegistrar;
    private readonly IServiceProvider _serviceProvider;
    public DefaultDbContextProvider(IDbContextServiceRegistrar dbContextServiceRegistrar
        , IServiceProvider serviceProvider,IConnectionStringResolver connectionStringResolver)
    {
        _dbContextServiceRegistrar = dbContextServiceRegistrar;
        _serviceProvider = serviceProvider;
      

    }

    public  Task<DbContext> GetDbContextAsync<TEntity>() where TEntity : class, IEntity
    {
        var dbContextType = _dbContextServiceRegistrar.EntityDbContextMapping[typeof(TEntity)];
        //var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(dbContextType);
        //var connectionString =await _connectionStringResolver.ResolveAsync(connectionStringName);
        return Task.FromResult((DbContext)_serviceProvider.GetRequiredService(dbContextType));
    }
}
