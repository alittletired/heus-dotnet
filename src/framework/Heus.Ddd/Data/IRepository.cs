using Heus.Core.DependencyInjection;

namespace Heus.Ddd.Data;

public interface IRepository<TEntity>: IScopedDependency where TEntity : class, IEntity
{

    Task<int> SaveAsync(TEntity entity);
    Task<IQueryable<TEntity>> GetQueryableAsync();
}
