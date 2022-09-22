using Heus.Core.DependencyInjection;

namespace Heus.DDD.Infrastructure;

public interface IUnitOfWorkManager : ISingletonDependency
{
    IUnitOfWork? Current { get; }
    IUnitOfWork Begin(UnitOfWorkOptions options, bool requiresNew = false);
}