using System.Data.Common;
namespace Heus.Data.Internal;

public class DbConnectionWrapper
{
    public required DbConnection DbConnection { get; init; }
    public required IDbConnectionProvider  DbConnectionProvider{ get; init; }
}
public interface IDbConnectionManager:IDisposable
{
    DbConnectionWrapper GetDbConnection<TContext>() where TContext : DbContext;
}
