using Heus.Core.Ddd.Data;
using Heus.Ddd.Data;
using Microsoft.EntityFrameworkCore;

namespace Heus.Ddd.Data;

internal class DefaultRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly DbContext _dbContext;
    public DefaultRepository(IDbContextProvider contextProvider)
    {
        _dbContext = contextProvider.GetDbContext(typeof(TEntity));
    }

    public IQueryable<TEntity> Query => _dbContext.Set<TEntity>();

    public async Task<int> SaveAsync(TEntity entity)
    {
        if (entity.Id == null)
        {
            entity.Id=EntityId.NewId();
            _dbContext.Add(entity);    
        }
        else
        {
            _dbContext.Update(entity);    
        }
        return await _dbContext.SaveChangesAsync();
    }
}