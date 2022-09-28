using Heus.Core.DependencyInjection;
using Heus.Ddd.Uow;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.EfCore.Internal
{
    internal class DbContextOptionsFactory : ISingletonDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IEnumerable<IDbContextOptionsProvider> _dbContextOptionsProviders;

        public DbContextOptionsFactory(
             IUnitOfWorkManager unitOfWorkManager
            , IEnumerable<IDbContextOptionsProvider> dbContextOptionsProviders)
        {
         
            _unitOfWorkManager= unitOfWorkManager;
            _dbContextOptionsProviders = dbContextOptionsProviders;
        }

        public DbContextOptions<TDbContext> Create<TDbContext>() where TDbContext : DbContext
        {

            return null!;
        }

    }
}
