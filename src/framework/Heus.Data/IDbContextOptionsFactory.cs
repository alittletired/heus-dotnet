

namespace Heus.Data;
internal interface IDbContextOptionsFactory
{
    DbContextOptions<TDbContext> Create<TDbContext>() where TDbContext : DbContext;
}
