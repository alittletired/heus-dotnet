using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.EfCore.Repositories;

internal class EfCoreRepositoryProvider<TEntity> :
    IRepositoryProvider<TEntity>, ISupportSaveChanges
    where TEntity : class, IEntity
{
    private readonly IDbContextProvider _dbContextProvider;
    private readonly DbContext _dbContext;

    public EfCoreRepositoryProvider(IDbContextProvider dbContextProvider)
    {
        _dbContextProvider = dbContextProvider;
        _dbContext = _dbContextProvider.GetDbContext<TEntity>();
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return GetDbSet().AsQueryable();
    }

    protected DbSet<TEntity> GetDbSet()
    {
        return _dbContext.Set<TEntity>();
    }

    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        TrySetGuidId(entity);
        var savedEntity = (await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken)).Entity;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return savedEntity;
    }



    protected virtual void TrySetGuidId(TEntity entity)
    {
        if (entity.Id != default)
        {
            return;
        }

        entity.Id = EntityId.NewId();
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {

        _dbContext.Attach(entity);
        var updatedEntity = _dbContext.Update(entity).Entity;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return updatedEntity;
    }


    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.AddRange(entities);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().UpdateRange(entities);
        await _dbContext.SaveChangesAsync(cancellationToken);

    }

    public async Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }


}