using System.Data.Common;

namespace Heus.DDD.Infrastructure.Internal;

internal class ChildUnitOfWork : IUnitOfWork
{
    private readonly IUnitOfWork _parent;

    
    public UnitOfWorkOptions Options => _parent.Options;

    public ChildUnitOfWork(IUnitOfWork parent)
    {
        _parent = parent;
    }

    public Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return _parent.RollbackAsync(cancellationToken);
    }

    public void Dispose()
    {

    }
}