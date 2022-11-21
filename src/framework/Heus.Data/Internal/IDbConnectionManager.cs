using System.Data.Common;
namespace Heus.Data.Internal;
internal interface IDbConnectionManager:IDisposable
{
    (DbConnection,DbProvider) GetDbConnection<TContext>() where TContext : DbContext;
}
