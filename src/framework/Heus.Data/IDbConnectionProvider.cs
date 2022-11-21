using System.Data.Common;
namespace Heus.Data;
public interface IDbConnectionProvider
{
    DbConnection CreateConnection(string connectionStringName);
    void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection);
    DbProviderFactory DbProviderFactory { get; }
    DbProvider DbProvider { get; }
}