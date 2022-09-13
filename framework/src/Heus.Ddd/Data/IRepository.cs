namespace Heus.Ddd.Data;

public interface IRepository<TEntity> where TEntity : IEntity
{
    IQueryable<TEntity> Query { get; }
    Task<int> SaveAsync(TEntity entity);
}
