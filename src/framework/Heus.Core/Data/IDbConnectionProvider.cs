using Heus.Core.Data.Options;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Heus.Core.Data;

public interface IDbConnectionProvider:IDisposable
{
    DbConnection CreateConnection(string connectionStringName);
    void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection);
    DbProvider DbProvider { get; }
}