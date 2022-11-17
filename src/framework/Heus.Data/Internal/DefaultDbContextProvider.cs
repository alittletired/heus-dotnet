using Heus.Core.DependencyInjection;
using Heus.Core.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heus.Data.Internal;

internal class DefaultDbContextProvider : IDbContextProvider,IScopedDependency
{
    private readonly IOptions<DataOptions> _options;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    public DefaultDbContextProvider( IOptions<DataOptions> options
        , IUnitOfWorkManager unitOfWorkManager)
    {
        _options = options;
        _unitOfWorkManager = unitOfWorkManager;
    }
    public  DbContext GetDbContext<TEntity>() 
    {
        var dbContextType = _options.Value.EntityDbContextMappings[typeof(TEntity)];
        //var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(dbContextType);
        //var connectionString =await _connectionStringResolver.ResolveAsync(connectionStringName);
        var unitOfWork = _unitOfWorkManager.Current;
        if (unitOfWork == null)
        {
              throw new BusinessException("A DbContext can only be created inside a unit of work!");
        }
        var dbContext= (DbContext)unitOfWork.ServiceProvider.GetRequiredService(dbContextType);
        unitOfWork.DbContexts.TryAdd(dbContext);
        return dbContext;
    }
}
