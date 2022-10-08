

using Heus.Core.DependencyInjection;
using Heus.Core.ObjectMapping;
using Heus.Core.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Heus.Ddd.Application;

public abstract class ApplicationService : IApplicationService, IInjectServiceProvider, IScopedDependency
{
    public  IServiceProvider ServiceProvider { get; set; } = null!;
    protected IObjectMapper Mapper => GetRequiredService<IObjectMapper>();
    protected ILogger Logger => GetRequiredService<ILoggerFactory>().CreateLogger(GetType());

    protected T GetRequiredService<T>() where T : class
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    protected ICurrentUser CurrentUser => GetRequiredService<ICurrentUser>();
}