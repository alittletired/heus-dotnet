using System.Linq.Expressions;
using Heus.Core;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Domain;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories.Filtering;
using Heus.Ddd.Uow;
using Heus.Ddd.Uow.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.Repositories;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
    , IInitialization where TEntity : class, IEntity
{
    protected IServiceProvider ServiceProvider { get; private set; } = null!;

    public virtual void Initialize(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;

    }

    protected IUnitOfWorkManager UnitOfWorkManager => ServiceProvider.GetRequiredService<UnitOfWorkManager>();
    protected IDataFilter DataFilter => ServiceProvider.GetRequiredService<IDataFilter>();
    public abstract Task<IQueryable<TEntity>> GetQueryableAsync();
    public abstract Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);


    public virtual async Task InsertManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await InsertAsync(entity, cancellationToken);
        }

        await SaveChangesAsync(cancellationToken);

    }

    protected virtual Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        if (UnitOfWorkManager.Current != null)
        {
            return UnitOfWorkManager.Current.SaveChangesAsync(cancellationToken);
        }

        return Task.CompletedTask;
    }

    public abstract Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await UpdateAsync(entity, cancellationToken);
        }

        await SaveChangesAsync(cancellationToken);
    }

    public abstract Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);


    public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await DeleteAsync(entity, cancellationToken);
        }

        await SaveChangesAsync(cancellationToken);
    }

    public async Task<TEntity?> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var query = await GetQueryableAsync();
        return await query.Where(predicate).FirstOrDefaultAsync(cancellationToken);
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

    public abstract Task DeleteAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

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