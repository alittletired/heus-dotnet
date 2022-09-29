using Heus.Core;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Uow;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

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
            var unitOfWork = _unitOfWorkManager.Current;
            if (unitOfWork == null)
            {
                throw new BusinessException("A DbContextOptions can only be created inside a unit of work!");
            }
            var connectionString=
            unitOfWork.
            return null!;
        }

    }
}
