using Heus.Ddd.Infrastructure;
using System.Data;

namespace Heus.DDD.Infrastructure;

public class UnitOfWorkOptions: IUnitOfWorkOptions
{
    public IsolationLevel? IsolationLevel { get; set; }
    public bool? IsTransactional { get; set; }

    /// <summary>
    /// Milliseconds
    /// </summary>
    public int? Timeout { get; set; }

}
