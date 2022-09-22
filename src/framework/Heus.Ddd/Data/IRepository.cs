namespace Heus.Ddd.Data;

public interface IRepository<TEntity> where TEntity : class, IEntity
{

    Task<int> SaveAsync(TEntity entity);
    Task<IQueryable<TEntity>> GetQueryableAsync();
}
