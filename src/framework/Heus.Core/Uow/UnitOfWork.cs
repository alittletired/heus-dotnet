using System.Data.Common;
using System.Reflection;
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
    private readonly List<Func<Task>> _completedHandlers  = new();
    public event EventHandler<UnitOfWorkEventArgs>? Disposed;
    public IServiceProvider ServiceProvider { get; }
    public UnitOfWorkOptions Options { get; }
    private readonly Dictionary<Type, DbContext> _dbContexts  = new();
    private readonly ILogger<UnitOfWork> _logger;
    private readonly Dictionary<string, DbTransaction> _dbTransactions = new();
    public  void OnCompleted(Func<Task> handler)
    {
        _completedHandlers.Add(handler);
    }

    public UnitOfWork(IServiceProvider serviceProvider, UnitOfWorkOptions options)
    {
        Options = options;
        ServiceProvider = serviceProvider;
        _logger = ServiceProvider.GetRequiredService<ILogger<UnitOfWork>>();
    }
    private static DbContext CreateDbContext<TContext>(IServiceProvider serviceProvider)
        where TContext : DbContext
    {
        var contextFactory = serviceProvider.GetRequiredService<IDbContextFactory<TContext>>();
        return contextFactory.CreateDbContext();
    }

    private readonly static MethodInfo GenericCreateDbContext = typeof(UnitOfWork)
        .GetTypeInfo().DeclaredMethods
        .First(m =>m.IsStatic && m.Name == nameof(CreateDbContext));
    
    // public DbContext GetDbContext(Type entityType)
    // {
    //   var contextResolver=  ServiceProvider.GetRequiredService<IDbContextResolver>();
    //   var dbContextType = contextResolver.Resolve(entityType);
    //
    //
    //
    // }
    public DbContext GetDbContext(Type dbContextType)
    {
        return _dbContexts.GetOrAdd(dbContextType, (type) =>
        {
            
            var activator = GenericCreateDbContext.MakeGenericMethod(type);
            var dbContext = activator.Invoke(null, new object[] { ServiceProvider });
            ArgumentNullException.ThrowIfNull(dbContext); 
            return (DbContext)dbContext;
        });
        
    }
  
   

    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        if (_isRollback)
        {
            return;
        }
        try
        {
          
            foreach (var dbContext in _dbContexts)
            {
                await dbContext.Value.SaveChangesAsync(cancellationToken);
            }
            foreach (var tran in _dbTransactions)
            {
                await tran.Value.CommitAsync(cancellationToken);
            }
            foreach (var handler in _completedHandlers)
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
            
                await dbTran.Value.RollbackAsync(cancellationToken);
           
        }

    }

    private void DisposeTransactions()
    {
        // return DbTransaction == null ? Task.CompletedTask : DbTransaction.ReleaseAsync();
        foreach (var dbTran in _dbTransactions)
        {
            dbTran.Value.Dispose();
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
        _dbContexts.ForEach(c => c.Value.Dispose());
        if (!_isCompleted || _exception != null)
        {
            Failed?.Invoke(this,new UnitOfWorkFailedEventArgs(this,_exception,_isRollback));
        }
        _dbContexts.Clear();
        Disposed?.Invoke(this, new UnitOfWorkEventArgs(this));
    }
}