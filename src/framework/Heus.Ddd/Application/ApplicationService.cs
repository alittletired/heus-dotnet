using Heus.Core.DependencyInjection;
using Heus.Core.ObjectMapping;
using Heus.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Heus.Ddd.Application;

public abstract class ApplicationService : IApplicationService, IInjectServiceProvider, IScopedDependency
{
    private IServiceProvider _serviceProvider=null!;
    private CachedServiceProvider _cachedServiceProvider = null!;
    
    protected IObjectMapper Mapper => GetRequiredService<IObjectMapper>();
    protected ILogger Logger => GetRequiredService<ILoggerFactory>().CreateLogger(GetType());

    protected T GetRequiredService<T>() where T : class
    {
        return _cachedServiceProvider.GetRequiredService<T>();
    }
    [NonAction]
    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _cachedServiceProvider = new(_serviceProvider);
    }

    protected ICurrentUser CurrentUser => GetRequiredService<ICurrentUser>();
}