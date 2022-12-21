using Microsoft.EntityFrameworkCore;

namespace Heus.Core.Uow;
public interface IUnitOfWork :  IDisposable
{
  
    UnitOfWorkOptions Options { get; }
  
    // Task EnsureTransaction(DbContext dbContext);
    event EventHandler<UnitOfWorkEventArgs>? Disposed;
    DbContext AddDbContext(string key, Func<string,DbContext> func);
    Task CompleteAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
    void OnCompleted(Func<Task> handler);
    event EventHandler<UnitOfWorkFailedEventArgs>? Failed;

}