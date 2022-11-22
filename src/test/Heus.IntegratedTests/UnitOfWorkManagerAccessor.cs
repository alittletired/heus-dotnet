using Heus.Core.Uow;

namespace Heus.IntegratedTests;

public static class UnitOfWorkManagerAccessor
{
    public static IUnitOfWorkManager UnitOfWorkManager { get; set; } = null!;
}
