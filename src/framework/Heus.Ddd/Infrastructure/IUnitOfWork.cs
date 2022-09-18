
namespace Heus.DDD.Infrastructure;
public interface IUnitOfWork:IDisposable
{
  
    UnitOfWorkOptions Options { get; }
    Task CompleteAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);

}