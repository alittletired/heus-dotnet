using System.Data.Common;

namespace Heus.Core.Uow;

internal class UnitOfWork : IUnitOfWork
{

    private bool _isDisposed;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(UnitOfWorkOptions options, ILogger<UnitOfWork> logger)
    {
        Options = options;
        _logger = logger;
    }

    public Dictionary<string, DbConnection> DbConnections { get; } = new();
    public Dictionary<string, DbTransaction> DbTransactions { get; } = new();
    public event EventHandler<UnitOfWorkEventArgs>? Disposed;
    public UnitOfWorkOptions Options { get; }

    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        foreach (var dbTran in DbTransactions.Values)
        {
            await dbTran.CommitAsync(cancellationToken);
        }

    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        foreach (var dbTran in DbTransactions)
        {
            try
            {
                await dbTran.Value.RollbackAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(RollbackAsync)}  Fail :{dbTran.Key}");
            }
        }

    }

    private void DisposeTransactions()
    {
        // return DbTransaction == null ? Task.CompletedTask : DbTransaction.ReleaseAsync();
        foreach (var dbTran in DbTransactions)
        {
            try
            {
                dbTran.Value.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(DisposeTransactions)}  Fail :{dbTran.Key}");

            }
        }

        DbTransactions.Clear();
    }



    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;
        DisposeTransactions();
        DbConnections.Values.ForEach(c => c.Dispose());
        Disposed?.Invoke(this, new UnitOfWorkEventArgs(this));
    }
}