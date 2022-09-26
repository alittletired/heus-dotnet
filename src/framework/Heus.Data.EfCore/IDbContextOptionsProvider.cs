using Heus.Core.DependencyInjection;
using Heus.Ddd.Data.Options;
using Microsoft.EntityFrameworkCore;

namespace Heus.Data.EfCore;

public interface IDbContextOptionsProvider : ISingletonDependency, IEnumableService
{
    Action<DbContextOptionsBuilder> OptionsBuilder { get; }
    DbProvider DbProvider { get; }
}
