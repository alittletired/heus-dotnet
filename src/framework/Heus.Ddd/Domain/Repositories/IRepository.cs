using Heus.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Heus.Ddd.Data;

public interface IRepository<TEntity> : IScopedDependency where TEntity : class, IEntity
{
    Task DeleteAsync(EntityId id);
    Task<int> SaveAsync(TEntity entity);
    Task<IQueryable<TEntity>> GetQueryableAsync();

    async Task<TEntity> GetByIdAsync(EntityId id)
    {
        var query = await GetQueryableAsync();
        return await query.FirstAsync(s => s.Id == id);
    }

    async Task<TEntity?> GetByIdOrDefaultAsync(EntityId id)
    {
        var query = await GetQueryableAsync();
        return await query.FirstOrDefaultAsync(s => s.Id == id);

    }
}
