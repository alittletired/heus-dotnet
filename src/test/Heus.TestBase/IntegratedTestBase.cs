using System.Security.Claims;
using Heus.Core.DependencyInjection;
using Heus.Core.DependencyInjection.Autofac;
using Heus.Core.Uow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Heus.TestBase;
//[TestScope]
public abstract class IntegratedTestBase<TStartupModule> : TestBaseWithServiceProvider,IAsyncLifetime where TStartupModule : IModuleInitializer
{
   
    protected virtual bool AutoCreateUow => true;
    private readonly DefaultModuleManager _moduleManager;

    protected IntegratedTestBase()
    {
        var serviceFactory = new AutofacServiceProviderFactoryFacade();
        var services = new ServiceCollection();
        var config = new ConfigurationManager();
        _moduleManager = new DefaultModuleManager(typeof(TStartupModule));
        _moduleManager.ConfigureServices(services, config);
        AfterConfigureServices(services);
        var containerBuilder = serviceFactory.CreateBuilder(services);
        RootServiceProvider = serviceFactory.CreateServiceProvider(containerBuilder);
        TestServiceScope = RootServiceProvider.CreateScope();
        ServiceProvider = TestServiceScope.ServiceProvider;
        UnitOfWorkManagerAccessor.UnitOfWorkManager = UnitOfWorkManager;
        UnitOfWorkManager.Begin();
    }
    protected virtual Task BeforeTestAsync() {
        return Task.CompletedTask;
    }
    public async Task InitializeAsync()
    {
        await _moduleManager.InitializeModulesAsync(ServiceProvider);
       await WithUnitOfWorkAsync(BeforeTestAsync);
      
    }

    public Task DisposeAsync()
    {
        TestServiceScope.Dispose();
        return Task.CompletedTask;
    }
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
    
    

    protected Task<TResult> WithUnitOfWorkAsync<TResult>(Func<Task<TResult>> func)
    {
        return WithUnitOfWorkAsync(new UnitOfWorkOptions(), func);
    }

    protected async Task<TResult> WithUnitOfWorkAsync<TResult>(UnitOfWorkOptions options, Func<Task<TResult>> func)
    {
        using (var uow = UnitOfWorkManager.Begin(options, true))
        {
            var result = await func();
            await uow.CompleteAsync();
            return result;
        }

    }

   

  
}