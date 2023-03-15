using System.Data;

namespace Heus.Ddd.Uow;
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
public class UnitOfWorkAttribute:Attribute
{
    /// <summary>
    /// Is this UOW transactional?
    /// Uses default value if not supplied.
    /// </summary>
    public bool? IsTransactional { get; set; }
    /// <summary>
    /// Timeout of UOW As milliseconds.
    /// Uses default value if not supplied.
    /// </summary>
    public int? Timeout { get; set; }
    /// <summary>
    /// If this UOW is transactional, this option indicated the isolation level of the transaction.
    /// Uses default value if not supplied.
    /// </summary>
    public IsolationLevel? IsolationLevel { get; set; }
    /// <summary>
    /// Used to prevent starting a unit of work for the method.
    /// If there is already a started unit of work, this property is ignored.
    /// Default: false.
    /// </summary>
    public bool IsDisabled { get; set; }
   
}