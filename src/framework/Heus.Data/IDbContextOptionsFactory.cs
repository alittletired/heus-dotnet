
namespace Heus.Data;

public interface IDbContextOptionsFactory<TContext> where TContext : DbContext
{
    DbContextOptions<TContext> CreateOptions();
}
