
namespace Heus.Data;

public interface IDbContextOptionsFactory
{
    DbContextOptions<TContext> CreateOptions<TContext>() where TContext : DbContext;
    DbContextOptions CreateOptions(Type contextType);
}
