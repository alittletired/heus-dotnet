using System.Data.Common;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using IsolationLevel = System.Data.IsolationLevel;

namespace Heus.Core.Uow;
internal class UnitOfWork : IUnitOfWork
{

    private bool _isDisposed;

    public event EventHandler<UnitOfWorkEventArgs>? Disposed;
    public IServiceProvider ServiceProvider { get; }
    public UnitOfWorkOptions Options { get; }
    public Dictionary<string, DbContext> DbContexts { get; } = new();
    private ILogger<UnitOfWork> _logger;
    private Dictionary<string, DbTransaction> _dbTransactions = new();


    public UnitOfWork(UnitOfWorkOptions options)
    {
        Options = options;
        ServiceProvider = options.ServiceProvider;
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
            dbTransaction = dbContext.Database.GetDbConnection().BeginTransaction(IsolationLevel.ReadCommitted);
            _dbTransactions.Add(connStr, dbTransaction);

        }

        dbContext.Database.UseTransactionAsync(dbTransaction);

    }

    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {

        try
        {
            foreach (var dbContext in DbContexts)
            {
                await dbContext.Value.SaveChangesAsync();
            }
            foreach (var tran in _dbTransactions)
            {
                await tran.Value.CommitAsync();
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
        foreach (var dbTran in _dbTransactions)
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
        if (_isDisposed) return;
        _isDisposed = true;
        DisposeTransactions();
        DbContexts.Values.ForEach(c => c.Dispose());
        Disposed?.Invoke(this, new UnitOfWorkEventArgs(this));
    }
}