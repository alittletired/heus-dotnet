using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Heus.DDD.Infrastructure;
public interface IUnitOfWork:IDisposable
{
  
    UnitOfWorkOptions Options { get; }
    DbTransaction? DbTransaction{ get; }
    Task CompleteAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);

}