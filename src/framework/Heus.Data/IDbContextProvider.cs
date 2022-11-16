namespace Heus.Data;

public interface IDbContextProvider
{
    DbContext GetDbContext<TEntity>();
}