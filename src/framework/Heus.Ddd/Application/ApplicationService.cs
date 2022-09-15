using Heus.Core.DependencyInjection;
using Heus.Core.Security;
using Heus.Ddd.Application;

namespace Heus.Ddd.Application;

public abstract class ApplicationService:IApplicationService
{
    public ILazyServiceProvider LazyServiceProvider { get; set; } = null!;
    protected ICurrentUser CurrentUser => LazyServiceProvider.LazyGetRequiredService<ICurrentUser>();
}