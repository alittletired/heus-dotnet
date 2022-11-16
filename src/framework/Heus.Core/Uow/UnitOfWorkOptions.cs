using System.Data;

namespace Heus.Core.Uow;

public class UnitOfWorkOptions: IUnitOfWorkOptions
{
    public UnitOfWorkOptions(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
    public IsolationLevel? IsolationLevel { get; set; }
    public bool? IsTransactional { get; set; }
    public IServiceProvider ServiceProvider { get;  } 

    /// <summary>
    /// Milliseconds
    /// </summary>
    public int? Timeout { get; set; }

}
