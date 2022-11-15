

using System.Data.Common;
using Heus.Core.DependencyInjection;

namespace Heus.Data.Internal;
internal interface IDbConnectionManager
{
    DbConnection GetDbConnection<TDbContext>() where TDbContext : DbContext;
}
