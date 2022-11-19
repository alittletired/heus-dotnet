using Heus.Core.DependencyInjection;

namespace Heus.Data.Uow;

public interface IUnitOfWorkManager : ISingletonDependency
{
    IUnitOfWork? Current { get; }
    IUnitOfWork Begin(UnitOfWorkOptions options, bool requiresNew = false);
}