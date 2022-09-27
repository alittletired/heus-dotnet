using Heus.Core.Ddd.Data;
using Heus.Ddd.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Data.EfCore.Internal;

internal class DefaultDbContextProvider: IDbContextProvider
{
    private readonly DbContextServiceRegistrar _dbContextServiceRegistrar;
    private readonly IServiceProvider _serviceProvider;
    private readonly IEnumerable<IDbContextOptionsProvider> _dbContextOptionsProviders;

    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public DefaultDbContextProvider(DbContextServiceRegistrar dbContextServiceRegistrar
        , IServiceProvider serviceProvider
        , IEnumerable<IDbContextOptionsProvider> dbContextOptionsProviders,IUnitOfWorkManager unitOfWorkManager)
    {
        _dbContextServiceRegistrar = dbContextServiceRegistrar;
        _serviceProvider = serviceProvider;
        _dbContextOptionsProviders = dbContextOptionsProviders;
        _unitOfWorkManager= unitOfWorkManager;
    }

    Task<DbContext> IDbContextProvider.GetDbContextAsync<TEntity>()
    {
        var dbContextType = _dbContextServiceRegistrar.EntityDbContexts[typeof(TEntity)];
       return    Task.FromResult((DbContext)_serviceProvider.GetRequiredService(dbContextType));
    }
}
