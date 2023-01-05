using System.Data;
namespace Heus.Core.Uow;

public interface IUnitOfWorkOptions
{
    public IsolationLevel? IsolationLevel { get;  }
    public bool IsTransactional { get;  }

    /// <summary>
    /// Milliseconds
    /// </summary>
    public int? Timeout { get;  }
}
