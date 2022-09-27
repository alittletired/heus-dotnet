using Heus.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.EfCore.Internal
{
    internal class DbContextOptionsFactory : IScopedDependency
    {
        public DbContextOptions<TDbContext> Create<TDbContext>() where TDbContext : DbContext
        {

            return null!;
        }

    }
}
