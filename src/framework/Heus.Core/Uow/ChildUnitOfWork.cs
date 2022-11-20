


using Microsoft.EntityFrameworkCore;

namespace Heus.Core.Uow;
internal class ChildUnitOfWork : IUnitOfWork
{
    private readonly IUnitOfWork _parent;

    public UnitOfWorkOptions Options => _parent.Options;
    public DbContext AddDbContext(string key, Func<string,DbContext> func) { 
        return _parent.AddDbContext(key, func);
        }
   
    public IServiceProvider ServiceProvider => _parent.ServiceProvider;
    public Task EnsureTransaction(DbContext dbContext) { 
        return _parent.EnsureTransaction(dbContext); 
        }
    public event EventHandler<UnitOfWorkEventArgs>? Disposed;
    
    public ChildUnitOfWork(IUnitOfWork parent)
    {
        _parent = parent;
        _parent.Disposed += (sender, args) => { Disposed?.Invoke(sender, args); };
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