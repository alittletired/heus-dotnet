using Heus.Core.Ddd.Data;
using Heus.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Ddd.Data;

public  class DefaultRepository<TEntity> : IRepository<TEntity>, IInitialization where TEntity : class, IEntity
{ 
    public void Initialize(IServiceProvider serviceProvider)
    {
        _dbContextProvider = serviceProvider.GetRequiredService<IDbContextProvider>();
    }
    
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


   
    public Task DeleteAsync(EntityId id)
    {
        throw new NotImplementedException();
    }

    public  async Task<int> SaveAsync(TEntity entity)
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