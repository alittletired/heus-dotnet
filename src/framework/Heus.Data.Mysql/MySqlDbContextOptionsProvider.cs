using Heus.Data.EfCore;
using Heus.Ddd.Data.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Data.Mysql
{
    internal class MySqlDbContextOptionsProvider : IDbContextOptionsProvider
    {
        public Action<DbContextOptionsBuilder> OptionsBuilder => optionsBuilder=>optionsBuilder.useMy

        public DbProvider DbProvider => DbProvider.MySql;
    }
}
