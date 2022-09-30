using System.Data.Common;
using Heus.Core.Data.Options;
using Heus.Core.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.EfCore;



public interface IDbContextOptionsProvider : ISingletonDependency, IEnumableService
{
    void Configure(DbContextOptionsBuilder dbContextOptions,DbConnection shareConnection);
    string ProviderName { get; }
}
