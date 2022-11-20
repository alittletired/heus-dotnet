using System.Data.Common;
namespace Heus.Data.Internal;
internal interface IDbConnectionManager:IDisposable
{
    DbConnection GetDbConnection<TContext>() where TContext : DbContext;
}
