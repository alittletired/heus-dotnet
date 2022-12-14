using Heus.Core.DependencyInjection;

namespace Heus.Core.Uow;

public interface IUnitOfWorkManager : ISingletonDependency
{
    IUnitOfWork? Current { get; }
    IUnitOfWork Begin(IServiceProvider serviceProvider,UnitOfWorkOptions? options=default, bool requiresNew = false);
}