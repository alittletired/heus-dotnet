using System.Linq.Expressions;
using Heus.Core.DependencyInjection;
using Heus.Core.Security;
using Heus.Core.Uow;
using Heus.Data;
using Heus.Ddd.Entities;
using Heus.Ddd.Internal;
using Heus.Ddd.Repositories.Filtering;

using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.Repositories;

public abstract class RepositoryBase<TEntity> :
    IRepository<TEntity>, IScopedDependency where TEntity : class, IEntity
{
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IDataFilter DataFilter => ServiceProvider.GetRequiredService<IDataFilter>();
    protected ICurrentUser CurrentUser => ServiceProvider.GetRequiredService<ICurrentUser>();
    protected  DbContext DbContext
    {
        get
        {
            return ServiceProvider.GetRequiredService<IDbContextProvider>().CreateDbContext<TEntity>();
        }
    }

    public IServiceProvider ServiceProvider {
        get {
            if (UnitOfWorkManager.Current == null)
            {
                throw new BusinessException("A Repository can only be created inside a unit of work!");
            }
            return UnitOfWorkManager.Current.ServiceProvider;

        }
    }
    public RepositoryBase(IUnitOfWorkManager unitOfWorkManager)
    {
        UnitOfWorkManager = unitOfWorkManager;
    }

    public IQueryable<TEntity> Query
    {
        get
        {
            var query = DbContext.Set<TEntity>().AsQueryable();
            if (UnitOfWorkManager.Current?.Options.IsTransactional == true)
            {
                return query;
            }
            return  query.AsNoTracking();
        }
    }

   
    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        BeforeInsert(entity);
        var entry = await DbContext.AddAsync(entity);
        return entry.Entity;
    }

    private void BeforeInsert(TEntity entity)
    {
        TrySetId(entity);
        if (entity is AuditEntity auditEntity)
        {
            auditEntity.CreatedDate = DateTimeOffset.Now;
            auditEntity.CreatedBy = CurrentUser.Id;
            BeforeUpdate(entity);
        }
    }
    protected virtual void TrySetId(TEntity entity)
    {
        if (entity.Id != default)
        {
            return;
        }

        entity.Id = SnowflakeId.Default.NextId();
    }
    private void BeforeUpdate(TEntity entity)
    {
        if (entity is AuditEntity auditEntity)
        {
            auditEntity.UpdateBy = CurrentUser.Id;
            auditEntity.UpdateDate = DateTimeOffset.Now;
        }
    }

    public async Task InsertManyAsync(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            BeforeInsert(entity);
        }
        await DbContext.AddRangeAsync(entities);
    }



    public Task<TEntity> UpdateAsync(TEntity entity)
    {
        DbContext.Attach(entity);
        BeforeUpdate(entity);
        DbContext.Update(entity);
        return Task.FromResult(entity);
    }

    public virtual Task UpdateManyAsync(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            BeforeUpdate(entity);
        }

        DbContext.UpdateRange(entities);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        DbContext.Remove(entity);
        return Task.CompletedTask;
    }


    public virtual Task DeleteManyAsync(IEnumerable<TEntity> entities)
    {
        DbContext.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public async Task<TEntity?> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await Query.FirstOrDefaultAsync(predicate);
    }



    protected TQueryable ApplyDataFilters<TQueryable>(TQueryable query)
        where TQueryable : IQueryable<TEntity>
    {
        return ApplyDataFilters<TQueryable, TEntity>(query);
    }

    protected virtual TQueryable ApplyDataFilters<TQueryable, TOtherEntity>(TQueryable query)
        where TQueryable : IQueryable<TOtherEntity>
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TOtherEntity)))
        {
            query = (TQueryable)query.WhereIf(DataFilter.IsEnabled<ISoftDelete>(),
                e => ((ISoftDelete)e!).IsDeleted == false);
        }

        return query;
    }


}