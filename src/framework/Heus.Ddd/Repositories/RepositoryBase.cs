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
    protected  IServiceProvider ServiceProvider { get; }
   
    protected IDataFilter DataFilter => ServiceProvider.GetRequiredService<IDataFilter>();
    protected ICurrentUser CurrentUser => ServiceProvider.GetRequiredService<ICurrentUser>();
    protected  DbContext DbContext => ServiceProvider.GetRequiredService<IDbContextProvider>().CreateDbContext<TEntity>();

    public RepositoryBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public IQueryable<TEntity> Query
    {
        get
        {
            var query = DbContext.Set<TEntity>().AsQueryable();
            return ApplyDataFilter(query);
            //if (UnitOfWorkManager.Current?.Options.IsTransactional == true)
            //{
            //    return query;
            //}
            //return  query.AsNoTracking();
        }
    }

    private IQueryable<TEntity> ApplyDataFilter(IQueryable<TEntity> queryable)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            return queryable.WhereIf(DataFilter.IsEnabled<ISoftDelete>(), e => !EF.Property<bool>(e,nameof(ISoftDelete.IsDeleted)));

        }

        return queryable;

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

}