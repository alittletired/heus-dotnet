using Heus.Core.Ddd.Data;
using Heus.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.Data;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity>, IOnInstantiation where TEntity : class, IEntity
{
    private IDbContextProvider _dbContextProvider = null!;
    protected async Task<DbContext> GetDbContextAsync()
    {
        return await _dbContextProvider.GetDbContextAsync(typeof(TEntity));
    }
    public  async Task<IQueryable<TEntity>> GetQueryableAsync()
    {
        var dbContext = await GetDbContextAsync();
        return dbContext.Set<TEntity>();
    }


    public void OnInstantiation(IServiceProvider serviceProvider)
    {
        _dbContextProvider = serviceProvider.GetRequiredService<IDbContextProvider>();
        //DbContext = contextProvider.GetDbContext(typeof(TEntity));
    }

    public virtual async Task<int> SaveAsync(TEntity entity)
    {
        var dbContext = await GetDbContextAsync();
        if (entity.Id == null)
        {
            entity.Id = EntityId.NewId();
            dbContext.Add(entity);
        }
        else
        {
            dbContext.Update(entity);
        }
        return await dbContext.SaveChangesAsync();
    }


}