
namespace Heus.Ddd.Uow;
public interface IUnitOfWork :  IDisposable
{
  
    UnitOfWorkOptions Options { get; }
    IServiceProvider ServiceProvider{ get; }
    // Task EnsureTransaction(DbContext dbContext);
    event EventHandler<UnitOfWorkEventArgs>? Disposed;

   
    DbContext GetDbContext(Type dbContextType);
    Task CompleteAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);
    void OnCompleted(Func<Task> handler);
    event EventHandler<UnitOfWorkFailedEventArgs>? Failed;

}