using System.Linq.Expressions;
using Heus.Ddd.Domain;
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Numerics;
using System;

namespace Heus.Ddd.Repositories;

public interface   IRepository<TEntity> : IRepository<TEntity, long> where TEntity: class, IEntity<long>
{

}
public interface IRepository<TEntity,TKey> where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
{
    IQueryable<TEntity> Query { get; }

    Task<TEntity> InsertAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);

    Task InsertManyAsync(IEnumerable<TEntity> entities);

    Task UpdateManyAsync(IEnumerable<TEntity> entities);

    Task DeleteManyAsync(IEnumerable<TEntity> entities);
   
    Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> predicate);

    Task<int> ExecuteUpdateAsync(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls);

    #region extensions
    async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Query.FirstOrDefaultAsync(predicate);
    }
    async Task<TEntity?> FindByIdAsync(TKey id)
    {
        return await Query.FirstOrDefaultAsync(s=>s.Id.Equals( id));
    }
    async Task<TEntity> GetByIdAsync(TKey id)
    {
      
        var entity = await FindOneAsync(s => s.Id.Equals(id));
        EntityNotFoundException.ThrowIfNull(entity,nameof(IEntity.Id),id);
        return entity;
    }
   
    async Task DeleteByIdAsync(TKey id) 
    {
        var entity = await FindOneAsync(s=> s.Id.Equals(id));
        if (entity == null) return;
        await DeleteAsync(entity);

    }

   async Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await Query.Where(filter).ToListAsync();
    }

   async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
   {
       return await Query.AnyAsync(filter);
   }
    async Task<bool> ExistsByIdAsync(TKey id)
    {
        return await ExistsAsync(s => s.Id.Equals(id));
    }
    #endregion
}
