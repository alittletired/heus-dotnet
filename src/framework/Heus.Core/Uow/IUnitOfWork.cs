
using Microsoft.EntityFrameworkCore;

namespace Heus.Core.Uow;
public interface IUnitOfWork :  IDisposable
{
  
    UnitOfWorkOptions Options { get; }
    IServiceProvider ServiceProvider { get; }
    event EventHandler<UnitOfWorkEventArgs>? Disposed;

    Task CompleteAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);

}