using Heus.Core.DependencyInjection;
using Heus.Core.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Heus.Data.Internal;

internal class DefaultDbContextProvider : IDbContextProvider, IScopedDependency
{
    private readonly IOptions<DataOptions> _options;
    public DefaultDbContextProvider(IOptions<DataOptions> options
        , IUnitOfWorkManager unitOfWorkManager)
    {
        _options = options;
       
    }

    public DbContext GetDbContext<TEntity>()
    {
        var dbContextType = _options.Value.EntityDbContextMappings[typeof(TEntity)];
        //var connectionStringName = ConnectionStringNameAttribute.GetConnStringName(dbContextType);
        //var connectionString =await _connectionStringResolver.ResolveAsync(connectionStringName);
       
      

        var dbContext = (DbContext)unitOfWork.ServiceProvider.GetRequiredService(dbContextType);
        unitOfWork.AddDbContext(dbContext);
        return dbContext;
    }
}
