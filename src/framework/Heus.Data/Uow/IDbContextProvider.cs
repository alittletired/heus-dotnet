
namespace Heus.Data.Uow;
public interface IDbContextProvider
{
    DbContext GetDbContext<TEntity>();
}