using System.Data;

namespace Heus.Ddd.Uow;

public class UnitOfWorkOptions : IUnitOfWorkOptions
{

    public IsolationLevel? IsolationLevel { get; set; }
    public bool IsTransactional { get; set; }

    /// <summary>
    /// Milliseconds
    /// </summary>
    public int? Timeout { get; set; }

    public UnitOfWorkOptions(UnitOfWorkAttribute? attribute = null)
    {
        if (attribute != null)
        {
            if (attribute.IsTransactional.HasValue)
            {
                IsTransactional =attribute. IsTransactional.Value;
            }
           
            Timeout =attribute. Timeout;
            IsolationLevel =attribute. IsolationLevel;

        }

    }
}
