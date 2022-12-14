using System.Security.Claims;
using Heus.Core.DependencyInjection;
using Heus.Core.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Heus.TestBase;
public class IntegratedTestBase<TStartupModule> : IAsyncLifetime, IDisposable where TStartupModule : IModuleInitializer
{
    protected IntegratedTestBase()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationManager();
        _moduleManager = new DefaultModuleManager(typeof(TStartupModule));
        _moduleManager.ConfigureServices(services, config);
        AfterConfigureServices(services);
        RootServiceProvider = services.BuildServiceProvider();
        TestServiceScope = RootServiceProvider.CreateScope();
        ServiceProvider = TestServiceScope.ServiceProvider;
        _moduleManager.InitializeModulesAsync(ServiceProvider).ConfigureAwait(false).GetAwaiter().GetResult();
    }

    private DefaultModuleManager _moduleManager;
    protected virtual void AfterConfigureServices(IServiceCollection services)
    {
        var claims = new List<Claim>() {
                                new Claim(ClaimTypes.NameIdentifier, "20221214"),
                                new Claim(ClaimTypes.Name, " integratedTester"),
                                
                            };

        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        Thread.CurrentPrincipal = claimsPrincipal;
    }
    protected IServiceScope TestServiceScope { get; }
    protected IServiceProvider RootServiceProvider { get; }
    protected IServiceProvider ServiceProvider { get; set; }

    protected virtual T? GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }

    protected virtual T GetRequiredService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    public virtual  Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task DisposeAsync()
    {
       
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        TestServiceScope.Dispose();
    }
}