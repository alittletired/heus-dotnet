using System.Data;

namespace Heus.DDD.Infrastructure;
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
    public virtual void SetOptions(UnitOfWorkOptions options)
    {
        if (IsTransactional.HasValue)
        {
            options.IsTransactional = IsTransactional.Value;
        }

        if (Timeout.HasValue)
        {
            options.Timeout = Timeout;
        }

        if (IsolationLevel.HasValue)
        {
            options.IsolationLevel = IsolationLevel;
        }
    }
}