using Heus.Data.Options;
using System.Data.Common;
namespace Heus.Data;
public interface IDbConnectionProvider : IDisposable
{
    DbConnection CreateConnection(string connectionStringName);
    void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection);
    DbProvider DbProvider { get; }
}