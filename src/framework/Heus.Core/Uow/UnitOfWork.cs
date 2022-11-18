using System.Collections.Generic;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Heus.Core.Uow;

internal class UnitOfWork : IUnitOfWork
{

    private bool _isDisposed;
    private readonly ILogger<UnitOfWork> _logger;
    public event EventHandler<UnitOfWorkEventArgs>? Disposed;
    public IServiceProvider ServiceProvider { get; }
   
    private Dictionary<string, DbTransaction> _dbDbTransactions = new();
    public UnitOfWork(UnitOfWorkOptions options, ILogger<UnitOfWork> logger)
    {
        Options = options;
        _logger = logger;
        ServiceProvider = options.ServiceProvider;
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
    public void AddDbContext(DbContext dbContext)
    {
        _dbContexts.TryAdd(dbContext);
    }
    private List<DbContext> _dbContexts = new();
    public IEnumerable<DbContext> GetDbContexts() {
        return _dbContexts;
        }
   
    public UnitOfWorkOptions Options { get; }
  
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