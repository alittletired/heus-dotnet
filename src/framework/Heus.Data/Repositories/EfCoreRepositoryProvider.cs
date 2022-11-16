
//using Heus.Ddd.Entities;
//namespace Heus.Data.EfCore.Repositories;

//internal class EfCoreRepositoryProvider<TEntity> :
//    IRepositoryProvider<TEntity>
//    where TEntity : class, IEntity
//{
//    private readonly IDbContextProvider _dbContextProvider;
//    private readonly DbContext _dbContext;

//    public EfCoreRepositoryProvider(IDbContextProvider dbContextProvider)
//    {
//        _dbContextProvider = dbContextProvider;
//        _dbContext = _dbContextProvider.GetDbContext<TEntity>();
//    }

//    public IQueryable<TEntity> Query => DbSet.AsQueryable().AsNoTracking();

//    protected DbSet<TEntity> DbSet => _dbContext.Set<TEntity>();


//    public async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
//    {
//        TrySetGuidId(entity);
//        var entry = await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
//        return entry.Entity;

//    }

//    protected virtual void TrySetGuidId(TEntity entity)
//    {
//        if (entity.Id != default)
//        {
//            return;
//        }

//        entity.Id = SnowflakeId.Default.NextId();
//    }

//    public  Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
//    {

//        _dbContext.Attach(entity);
//        var entityEntry = _dbContext.Update(entity);
//        return Task.FromResult( entityEntry.Entity);
//    }


//    public  Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
//    {
//        _dbContext.Set<TEntity>().Remove(entity);
//        return Task.CompletedTask;
//    }



//    public async Task InsertManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
//    {
//       await _dbContext.AddRangeAsync(entities);
      
//    }

//    public  Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
//    {
//        _dbContext.UpdateRange(entities);
//        return Task.CompletedTask;
    

//    }

//    public Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
//    {
//        _dbContext.RemoveRange(entities);
//        return Task.CompletedTask;
//    }


//}