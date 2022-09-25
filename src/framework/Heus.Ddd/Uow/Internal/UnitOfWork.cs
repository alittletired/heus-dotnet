using System.Data.Common;

namespace Heus.Ddd.Uow.Internal;

internal class UnitOfWork : IUnitOfWork
{

    private bool _isDisposed = false;

    public UnitOfWork(UnitOfWorkOptions options)
    {
        Options = options;
    }

    public event EventHandler<UnitOfWorkEventArgs>? Disposed;
    public UnitOfWorkOptions Options { get; }
    public DbTransaction? DbTransaction { get; }

    public Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        return DbTransaction == null ? Task.CompletedTask : DbTransaction.CommitAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return DbTransaction == null ? Task.CompletedTask : DbTransaction.RollbackAsync(cancellationToken);
    }

    private Task DisposeTransactions()
    {
        // return DbTransaction == null ? Task.CompletedTask : DbTransaction.ReleaseAsync();
        return Task.CompletedTask;
    }


    public void  Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;
        // await DisposeTransactions();
        Disposed?.Invoke(this, new UnitOfWorkEventArgs(this));
    }
}