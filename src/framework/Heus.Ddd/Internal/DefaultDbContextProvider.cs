
using System.Reflection;
using Heus.Core.DependencyInjection;
using Heus.Core.Uow;
using Heus.Core.Utils;
using Heus.Ddd.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
namespace Heus.Ddd.Internal;

internal class DefaultDbContextProvider : IDbContextProvider, IScopedDependency
{
    private readonly IOptions<DddOptions> _options;
    private readonly IUnitOfWorkManager _unitOfWorkManager;
    public DefaultDbContextProvider(IOptions<DddOptions> options
        , IUnitOfWorkManager unitOfWorkManager)
    {
        _options = options;
        _unitOfWorkManager = unitOfWorkManager;

    }
    private static DbContext CreateDbContextInternal<TContext>(IServiceProvider serviceProvider)
        where TContext : DbContext
    {
        var contextFactory = serviceProvider.GetRequiredService<IDbContextFactory<TContext>>();
        return contextFactory.CreateDbContext();
    }

    private static readonly MethodInfo _createDbContext = typeof(DefaultDbContextProvider)
        .GetTypeInfo().DeclaredMethods
        .First(m => m.Name == nameof(CreateDbContextInternal));

    public DbContext CreateDbContext<TEntity>() where TEntity : IEntity
    {
        if (_unitOfWorkManager.Current == null)
        {
            throw new BusinessException("A DbContext can only be created inside a unit of work!");
        }
        var dbContextType = _options.Value.EntityDbContextMappings[typeof(TEntity)];
        return _unitOfWorkManager.Current.AddDbContext(dbContextType.Name, (key) =>
        {
            var activator = _createDbContext.MakeGenericMethod(dbContextType);
            var dbContext = activator.Invoke(null, new object[] { _unitOfWorkManager.Current.ServiceProvider });
           ArgumentNullException.ThrowIfNull(dbContext); 
            return (DbContext)dbContext;
        });

    }

}
