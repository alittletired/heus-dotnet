using System.Data;

namespace Heus.Core.Uow;

public class UnitOfWorkOptions: IUnitOfWorkOptions
{
    public IsolationLevel? IsolationLevel { get; set; }
    public bool? IsTransactional { get; set; }

    /// <summary>
    /// Milliseconds
    /// </summary>
    public int? Timeout { get; set; }

}