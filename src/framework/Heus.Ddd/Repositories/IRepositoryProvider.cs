using System.Linq.Expressions;
using Heus.Ddd.Entities;

namespace Heus.Ddd.Repositories;

public interface IRepositoryProvider<TEntity> where TEntity : class, IEntity
{
    IQueryable<TEntity> Query { get; }
  
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task InsertManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    Task UpdateManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    Task DeleteManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);
    
}