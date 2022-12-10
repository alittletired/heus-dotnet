using Heus.Core.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.TestBase;

public class UnitTest<TStartupModule>:IAsyncLifetime  where TStartupModule : IModuleInitializer
{
    protected UnitTest()
    {
        var services =new ServiceCollection();
        var config = new ConfigurationManager();
        _moduleManager = new DefaultModuleManager(typeof(TStartupModule));
        _moduleManager.ConfigureServices(services,config);
     
        
        RootServiceProvider= services.BuildServiceProvider();
        TestServiceScope = RootServiceProvider.CreateScope();
       
    }

    private DefaultModuleManager _moduleManager;
    protected IServiceScope TestServiceScope { get; }
    protected IServiceProvider RootServiceProvider { get; }
    protected IServiceProvider ServiceProvider { get; set; } = null!;

    protected virtual T? GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }

    protected virtual T GetRequiredService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    public async Task InitializeAsync()
    {
        ServiceProvider = TestServiceScope.ServiceProvider;
        await _moduleManager.InitializeModulesAsync(ServiceProvider);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}