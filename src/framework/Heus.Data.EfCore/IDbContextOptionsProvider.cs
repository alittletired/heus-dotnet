using System.Data.Common;
using Heus.Core.DependencyInjection;
using Heus.Ddd.Data.Options;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.EfCore;



public interface IDbContextOptionsProvider : ISingletonDependency, IEnumableService
{
    void Configure(DbContextOptionsBuilder dbContextOptions,DbConnection shareConnection);
    DbProvider DbProvider { get; }
}
