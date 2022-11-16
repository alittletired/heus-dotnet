using System.Linq.Expressions;
using Heus.Ddd.Domain;
using Heus.Ddd.Entities;
namespace Heus.Ddd.Repositories;

public interface IRepository<TEntity> where TEntity : class, IEntity
{
    IQueryable<TEntity> Query { get; }

    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);

    Task InsertManyAsync(IEnumerable<TEntity> entities);

    Task UpdateManyAsync(IEnumerable<TEntity> entities);

    Task DeleteManyAsync(IEnumerable<TEntity> entities);
    Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate);
    async Task<TEntity> GetByIdAsync(long id)
    {
        var entity = await FindOneAsync(s=>s.Id==id);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }

        return entity;
    }
   
    async Task DeleteByIdAsync(long id) 
    {
        var entity = await FindOneAsync(s=>s.Id==id);
        if (entity == null) return;
        await DeleteAsync(entity);

    }

   async Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await Query.Where(filter).ToListAsync();
    }

   async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
   {
       return (await FindOneAsync(filter)) != null;
   }
}
