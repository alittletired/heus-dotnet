using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Heus.Core.Uow;
internal class UnitOfWork : IUnitOfWork
{
    private Exception? _exception;
    
    private bool _isDisposed;
    private bool _isCompleted;
    private bool _isRollback;
    
    public event EventHandler<UnitOfWorkFailedEventArgs>? Failed;
    protected List<Func<Task>> CompletedHandlers { get; } = new();
    public event EventHandler<UnitOfWorkEventArgs>? Disposed;
    public IServiceProvider ServiceProvider { get; }
    public UnitOfWorkOptions Options { get; }
    public Dictionary<string, DbContext> DbContexts { get; } = new();
    private readonly ILogger<UnitOfWork> _logger;
    private readonly Dictionary<string, DbTransaction> _dbTransactions = new();
    public  void OnCompleted(Func<Task> handler)
    {
        CompletedHandlers.Add(handler);
    }

    public UnitOfWork(IServiceProvider serviceProvider, UnitOfWorkOptions options)
    {
        Options = options;
        ServiceProvider = serviceProvider;
        _logger = ServiceProvider.GetRequiredService<ILogger<UnitOfWork>>();
    }

    public DbContext AddDbContext(string key, Func<string, DbContext> func)
    {
        return DbContexts.GetOrAdd(key, (t) =>
        {
            var context = func(t);
            EnsureTransaction(context);
            return context;
        });
    }

    private void EnsureTransaction(DbContext dbContext)
    {
        if (!Options.IsTransactional)
        {
            return;
        }

        if (dbContext.Database.CurrentTransaction != null)
            return;
        var connStr = dbContext.Database.GetConnectionString();
        if (connStr == null) return;
        if (!_dbTransactions.TryGetValue(connStr, out var dbTransaction))
        {
            dbTransaction = dbContext.Database.GetDbConnection()
                .BeginTransaction();
            _dbTransactions.Add(connStr, dbTransaction);
        }

        dbContext.Database.UseTransactionAsync(dbTransaction);

    }

    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        if (_isRollback)
        {
            return;
        }
        try
        {
          
            foreach (var dbContext in DbContexts)
            {
                await dbContext.Value.SaveChangesAsync(cancellationToken);
            }
            foreach (var tran in _dbTransactions)
            {
                await tran.Value.CommitAsync(cancellationToken);
            }
            foreach (var handler in CompletedHandlers)
            {
                await handler.Invoke();
            }
            _isCompleted = true;
           
        }
        catch (Exception ex)
        {
            _exception = ex;
            throw;
        }

    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_isRollback)
        {
            return;
        }
        _isRollback = true;
        foreach (var dbTran in _dbTransactions)
        {
            try
            {
                await dbTran.Value.RollbackAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(RollbackAsync)}  Fail :{dbTran.Key}");
                throw;
            }
        }

    }

    private void DisposeTransactions()
    {
        // return DbTransaction == null ? Task.CompletedTask : DbTransaction.ReleaseAsync();
        foreach (var dbTran in _dbTransactions)
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

        _dbTransactions.Clear();
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _isDisposed = true;
        DisposeTransactions();
        DbContexts.Values.ForEach(c => c.Dispose());
        if (!_isCompleted || _exception != null)
        {
            Failed?.Invoke(this,new UnitOfWorkFailedEventArgs(this,_exception,_isRollback));
        }

        Disposed?.Invoke(this, new UnitOfWorkEventArgs(this));
    }
}