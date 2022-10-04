using System.Linq.Expressions;
using Heus.Core.DependencyInjection;
using Heus.Core.Security;
using Heus.Ddd.Domain;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories.Filtering;
using Heus.Ddd.Uow;
using Heus.Ddd.Uow.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Heus.Ddd.Repositories;

public class DefaultRepository<TEntity> : IRepository<TEntity>
    , IInjectServiceProvider where TEntity : class, IEntity
{
    public IServiceProvider ServiceProvider { get; set; } = null!;
    protected IUnitOfWorkManager UnitOfWorkManager => ServiceProvider.GetRequiredService<UnitOfWorkManager>();
    protected IDataFilter DataFilter => ServiceProvider.GetRequiredService<IDataFilter>();

    protected ICurrentUser CurrentUser => ServiceProvider.GetRequiredService<ICurrentUser>();

    protected IRepositoryProvider<TEntity> RepositoryProvider =>
        ServiceProvider.GetRequiredService<IRepositoryProvider<TEntity>>();

    public IQueryable<TEntity> GetQueryable()
    {
        return RepositoryProvider.GetQueryable();
    }

    public Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        BeforeInsert(entity);
        return RepositoryProvider.InsertAsync(entity, cancellationToken);
    }

    private void BeforeInsert(TEntity entity)
    {
        if (entity is AuditEntity auditEntity)
        {
            auditEntity.CreatedDate=DateTime.UtcNow;
            auditEntity.CreatedBy = CurrentUser.UserId;
            BeforeUpdate(entity);
        }
    }

    private void BeforeUpdate(TEntity entity)
    {
        if (entity is AuditEntity auditEntity)
        {
            auditEntity.UpdateBy = CurrentUser.UserId;
            auditEntity.UpdateDate=DateTime.UtcNow;
        }
    }
    public   Task InsertManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            BeforeInsert(entity);
        }

        return RepositoryProvider.InsertManyAsync(entities, cancellationToken);
    }



    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        BeforeUpdate(entity);
        return RepositoryProvider.UpdateAsync(entity, cancellationToken);
    }

    public virtual  Task UpdateManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            BeforeUpdate(entity);
        }

        return RepositoryProvider.UpdateManyAsync(entities, cancellationToken);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return RepositoryProvider.DeleteAsync(entity, cancellationToken);
    }


    public virtual  Task DeleteManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        return RepositoryProvider.DeleteManyAsync(entities, cancellationToken);
    }

    public async Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {

        return await GetQueryable().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<TEntity> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var entity = await FindAsync(predicate, cancellationToken);
        if (entity == null)
        {
            throw new EntityNotFoundException(typeof(TEntity));
        }

        return entity;
    }


    protected virtual TQueryable ApplyDataFilters<TQueryable>(TQueryable query)
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