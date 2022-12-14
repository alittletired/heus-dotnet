using System.Data.Common;
namespace Heus.Data.Internal;
public interface IDbConnectionManager:IDisposable
{
    (DbConnection,DbProvider) GetDbConnection<TContext>() where TContext : DbContext;
}
