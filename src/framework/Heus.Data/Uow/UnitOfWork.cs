using System.Collections.Generic;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Heus.Data.Uow;

internal class UnitOfWork : IUnitOfWork
{

    private bool _isDisposed;
    
    public event EventHandler<UnitOfWorkEventArgs>? Disposed;
    public IServiceProvider ServiceProvider { get; }
    public UnitOfWorkOptions Options { get; }
    private Dictionary<string,DbContext> _dbContexts = new();
    private ILogger<UnitOfWork> _logger;
    private Dictionary<string, DbTransaction> _dbTransactions = new();
    public DbContext GetDbContext<TEntity>()
    {
        var provider = ServiceProvider.GetRequiredService<IDbContextProvider>();
        return provider.GetDbContext<TEntity>();
    }

    private Dictionary<string, DbTransaction> _dbDbTransactions = new();
    public UnitOfWork(UnitOfWorkOptions options)
    {
        Options = options;
        ServiceProvider = options.ServiceProvider;
        _logger = ServiceProvider.GetRequiredService<ILogger<UnitOfWork>>();
    }
    public async Task EnsureTransaction(DbContext dbContext)
    {
        if (Options.IsTransactional) {
            if (_transations.TryGetValue(connection, out var dbTransaction))
            {
                await dbContext.Database.UseTransactionAsync(dbTransaction);
                return;
            }
            var transation = await dbContext.Database.BeginTransactionAsync();
            _transations.Add(connection, transation.GetDbTransaction());
        }
    }
  
    
    public IEnumerable<DbContext> GetDbContexts() {
        return _dbContexts;
        }
   
  

    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        //foreach (var dbTran in DbTransactions.Values)
        //{
        //    await dbTran.CommitAsync(cancellationToken);
        //}
        try {
            foreach (var dbContext in _dbContexts)
            {
                await dbContext.SaveChangesAsync();
            }
            foreach (var tran in _transations)
            {
            await    tran.Value.CommitAsync();
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
        foreach (var dbTran in _transations)
        {
            try
            {
                await dbTran.Value.RollbackAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(RollbackAsync)}  Fail :{dbTran.Key.ConnectionString}");
            }
        }

    }

    private void DisposeTransactions()
    {
        // return DbTransaction == null ? Task.CompletedTask : DbTransaction.ReleaseAsync();
        foreach (var dbTran in _transations)
        {
            try
            {
                dbTran.Value.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(DisposeTransactions)}  Fail :{dbTran.Key?.ConnectionString}");

            }
        }

        _transations.Clear();
    }



    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;
        DisposeTransactions();
    
        Disposed?.Invoke(this, new UnitOfWorkEventArgs(this));
    }
}