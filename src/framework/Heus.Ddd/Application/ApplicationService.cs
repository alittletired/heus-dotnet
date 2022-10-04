

namespace Heus.Ddd.Application;

public abstract class ApplicationService : ServiceBase,IApplicationService 
{
    public override IServiceProvider ServiceProvider { get; set; } = null!;

}