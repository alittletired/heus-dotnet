using System.Linq.Expressions;
using Heus.Ddd.Domain;
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore.Query;

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
    Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate);

    Task<int> ExecuteUpdateAsync(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls);
    async Task<TEntity> GetByIdAsync(long id)
    {
        var entity = await FindOneAsync(s=>s.Id==id);
        EntityNotFoundException.ThrowIfNull(entity,nameof(IEntity.Id),id);
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
