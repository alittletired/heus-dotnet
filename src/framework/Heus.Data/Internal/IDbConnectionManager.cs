using System.Data.Common;
namespace Heus.Data.Internal;
internal interface IDbConnectionManager
{
    DbConnection GetDbConnection<TContext>() where TContext : DbContext;
}
