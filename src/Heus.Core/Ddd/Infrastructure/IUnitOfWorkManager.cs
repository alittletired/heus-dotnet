namespace Heus.DDD.Infrastructure;

public interface IUnitOfWorkManager
{
    IUnitOfWork? Current { get; }
    IUnitOfWork Begin( UnitOfWorkOptions options, bool requiresNew = false);
}