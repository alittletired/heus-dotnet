using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Heus.Core.Uow;

internal class UnitOfWork : IUnitOfWork
{

    private bool _isDisposed;
    private readonly ILogger<UnitOfWork> _logger;
    public event EventHandler<UnitOfWorkEventArgs>? Disposed;
    public IServiceProvider ServiceProvider { get; }

    public UnitOfWork(UnitOfWorkOptions options, ILogger<UnitOfWork> logger)
    {
        Options = options;

        _logger = logger;
        ServiceProvider = options.ServiceProvider;
    }

    public List<DbContext> DbContexts { get; } = new();
   
   
    public UnitOfWorkOptions Options { get; }
    private List<DbTransaction> _transactions=new();
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        //foreach (var dbTran in DbTransactions.Values)
        //{
        //    await dbTran.CommitAsync(cancellationToken);
        //}
        try {
            foreach (var dbContext in DbContexts)
            {
                await dbContext.SaveChangesAsync();
            }
            foreach (var tran in _transactions)
            {
                tran.Commit();
            }   
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
      
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        foreach (var dbTran in _transactions)
        {
            try
            {
                await dbTran.RollbackAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(RollbackAsync)}  Fail :{dbTran.Connection?.ConnectionString}");
            }
        }

    }

    private void DisposeTransactions()
    {
        // return DbTransaction == null ? Task.CompletedTask : DbTransaction.ReleaseAsync();
        foreach (var dbTran in _transactions)
        {
            try
            {
                dbTran.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(DisposeTransactions)}  Fail :{dbTran.Connection?.ConnectionString}");

            }
        }

        _transactions.Clear();
    }



    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;
        DisposeTransactions();
    
        Disposed?.Invoke(this, new UnitOfWorkEventArgs(this));
    }
}