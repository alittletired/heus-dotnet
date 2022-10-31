

using System.Data.Common;

namespace Heus.Core.Uow;
public interface IUnitOfWork :  IDisposable
{
  
    UnitOfWorkOptions Options { get; }

    Dictionary<string, DbConnection> DbConnections
    {
        get;
    }
    Task CompleteAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync(CancellationToken cancellationToken = default);

}