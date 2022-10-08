using System.Data.Common;
using Heus.Core.Data.Options;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.EfCore;



public interface IDbContextOptionsProvider 
{
    void Configure(DbContextOptionsBuilder dbContextOptions,DbConnection shareConnection);
    DbProvider DbProvider { get; }
    DbConnection CreateDbConnection(string connectionString);
}
