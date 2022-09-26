using Heus.Ddd.Data.Options;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.EfCore.Internal
{
    internal class DbContextOptionsManager: IDbContextOptionsManager
    {
        private readonly IEnumerable<IDbContextOptionsProvider> _dbContextOptions;
        public DbContextOptionsManager(IEnumerable<IDbContextOptionsProvider> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }
        public Action<DbContextOptionsBuilder> GetContextOptionsBuilder(DbProvider dbProvider)
        {
            return _dbContextOptions.First(s => s.DbProvider == dbProvider).OptionsBuilder;
        }
    }
}
