using System.Data;

namespace Heus.DDD.Infrastructure;

public class UnitOfWorkOptions
{
    public IsolationLevel? IsolationLevel { get; set; }
    public bool? IsTransactional { get; set; }

    /// <summary>
    /// Milliseconds
    /// </summary>
    public int? Timeout { get; set; }

    public IServiceProvider ServiceProvider { get; }

    public UnitOfWorkOptions(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

}
