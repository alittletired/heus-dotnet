
using System.Data.Common;

namespace Heus.Ddd.Uow;
public interface IUnitOfWork :  IDisposable
{
  
    UnitOfWorkOptions Options { get; }
    Task CompleteAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);

}