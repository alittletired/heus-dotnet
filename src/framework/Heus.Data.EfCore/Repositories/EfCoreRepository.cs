using System.Linq.Expressions;
using Heus.Ddd.Data;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.EfCore.Repositories;

public class EfCoreRepository<TEntity>: RepositoryBase<TEntity>
    where TEntity:class,IEntity
{
    private  readonly IDbContextProvider _dbContextProvider;

    public EfCoreRepository(IDbContextProvider dbContextProvider)
    {
        _dbContextProvider = dbContextProvider;
    }

    public override async Task<IQueryable<TEntity>> GetQueryableAsync()
    {
        return (await GetDbSetAsync()).AsQueryable();
    }
    public async Task<DbContext> GetDbContextAsync()
    {
       return await _dbContextProvider.GetDbContextAsync<TEntity>();
    }
    protected async Task<DbSet<TEntity>> GetDbSetAsync()
    {
        return (await GetDbContextAsync()).Set<TEntity>();
    }
    public override async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        TrySetGuidId(entity);
        var dbContext = await GetDbContextAsync();

        var savedEntity = (await dbContext.Set<TEntity>().AddAsync(entity, cancellationToken)).Entity;
        await dbContext.SaveChangesAsync(cancellationToken);
        return savedEntity;
    }

    public override async Task InsertManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var entityArray = entities.ToArray();
        foreach (var entity in entityArray)
        {
            TrySetGuidId(entity);
        }

        await dbContext.Set<TEntity>().AddRangeAsync(entityArray, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    protected virtual void TrySetGuidId(TEntity entity)
    {
        if (entity.Id != default)
        {
            return;
        }

        entity.Id = EntityId.NewId();
    }

    public override async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        dbContext.Attach(entity);

        var updatedEntity = dbContext.Update(entity).Entity;

        await dbContext.SaveChangesAsync(cancellationToken);
        return updatedEntity;
    }

    public override async Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        dbContext.Set<TEntity>().UpdateRange(entities);
        await dbContext.SaveChangesAsync(cancellationToken);

    }

    public override async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public override async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var dbSet = dbContext.Set<TEntity>();

        var entities = await dbSet
            .Where(predicate)
            .ToListAsync(cancellationToken);
        await DeleteManyAsync(entities, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

    }

    protected override async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
       await  (await GetDbContextAsync()).SaveChangesAsync(cancellationToken);
    }
}