
using Heus.Core.Uow;

namespace Heus.TestBase;
internal class UnitOfWorkManagerAccessor
{
    public static IUnitOfWorkManager UnitOfWorkManager { get; set; } = null!;
}
