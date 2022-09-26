using Heus.Ddd.Data.Options;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.EfCore
{
    public interface IDbContextOptionsManager
    {
        Action<DbContextOptionsBuilder> GetContextOptionsBuilder(DbProvider dbProvider);
    }
}
