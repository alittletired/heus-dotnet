namespace Heus.Data.Uow;
public interface IUnitOfWork :  IDisposable
{
  
    UnitOfWorkOptions Options { get; }
    IServiceProvider ServiceProvider { get; }
    event EventHandler<UnitOfWorkEventArgs>? Disposed;
    DbContext GetDbContext<TEntity>();
    Task CompleteAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);

}