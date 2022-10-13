using Heus.Core.Data.Options;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Heus.Core.Data;

public interface IDbConnectionProvider:IDisposable
{
    DbConnection CreateConnection(string connectionStringName);
    void Configure(DbContextOptionsBuilder dbContextOptions, DbConnection shareConnection);
    DbProvider DbProvider { get; }
}