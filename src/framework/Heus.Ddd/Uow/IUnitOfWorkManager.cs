using Heus.Core.DependencyInjection;

namespace Heus.Ddd.Uow;

public interface IUnitOfWorkManager : ISingletonDependency
{
    IUnitOfWork? Current { get; }
    IUnitOfWork Begin(UnitOfWorkOptions options, bool requiresNew = false);
}