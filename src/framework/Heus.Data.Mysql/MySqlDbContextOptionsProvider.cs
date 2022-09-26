using Heus.Data.EfCore;
using Heus.Ddd.Data.Options;
using Microsoft.EntityFrameworkCore;


namespace Heus.Data.Mysql
{
    internal class MySqlDbContextOptionsProvider : IDbContextOptionsProvider
    {
        public Action<DbContextOptionsBuilder> OptionsBuilder => optionsBuilder=>optionsBuilder.UseMySql

        public DbProvider DbProvider => DbProvider.MySql;
    }
}
