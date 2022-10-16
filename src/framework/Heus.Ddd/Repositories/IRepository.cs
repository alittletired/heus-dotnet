using System.Linq.Expressions;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Domain;
using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heus.Ddd.Repositories;

public interface ISupportSaveChanges
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
}

public interface IRepository<TEntity> : IRepositoryProvider<TEntity> where TEntity : class, IEntity
{

    Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// Get a single entity by the given <paramref name="predicate"/>.
    /// <para>
    /// It throws <see cref="EntityNotFoundException"/> if there is no entity with the given <paramref name="predicate"/>.
    /// It throws <see cref="InvalidOperationException"/> if there are multiple entities with the given <paramref name="predicate"/>.
    /// </para>
    /// </summary>
    /// <param name="predicate">A condition to filter entities</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default
    );
    /// <summary>
    ///  Get a single entity by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException"></exception>
    async Task<TEntity> GetByIdAsync(long id)
    {
        var entity = await GetByIdOrDefaultAsync(id);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity), id);
        }

        return entity;
    }

    async Task<TEntity?> GetByIdOrDefaultAsync(long id)
    {
      
        return await GetQueryable().FirstOrDefaultAsync(s => s.Id == id);

    }
    async Task DeleteByIdAsync(long id,
            CancellationToken cancellationToken = default) 
    {
        var entity = await GetByIdOrDefaultAsync(id);
        if (entity == null) return;
        await DeleteAsync(entity, cancellationToken);

    }

   async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default)
    {
        return await GetQueryable().Where(filter).ToListAsync(cancellationToken);
    }

   async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
   {
       return (await FindAsync(filter)) != null;
   }
}
