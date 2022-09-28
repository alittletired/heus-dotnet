using Heus.Core.Ddd.Data;
using Heus.Ddd.Data;
using Heus.Ddd.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.EfCore.Internal;

internal class DefaultDbContextProvider : IDbContextProvider
{
    private readonly IDbContextServiceRegistrar _dbContextServiceRegistrar;
    private readonly IServiceProvider _serviceProvider;

    public DefaultDbContextProvider(IDbContextServiceRegistrar dbContextServiceRegistrar
        , IServiceProvider serviceProvider)
    {
        _dbContextServiceRegistrar = dbContextServiceRegistrar;
        _serviceProvider = serviceProvider;

    }

    public Task<DbContext> GetDbContextAsync<TEntity>() where TEntity : class, IEntity
    {
        var dbContextType = _dbContextServiceRegistrar.EntityDbContextMapping[typeof(TEntity)];
        return Task.FromResult((DbContext)_serviceProvider.GetRequiredService(dbContextType));
    }
}
