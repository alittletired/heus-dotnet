using System.Security.Claims;
using Heus.Core.DependencyInjection;
using Heus.Core.DependencyInjection.Autofac;
using Heus.Core.Uow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Heus.TestBase;
public abstract class IntegratedTestBase<TStartupModule> :  IDisposable where TStartupModule : IModuleInitializer
{
   
    protected virtual bool AutoCreateUow => true;
    protected IntegratedTestBase()
    {
        var serviceFactory = new AutofacServiceProviderFactoryFacade();
        var services = new ServiceCollection();
        var config = new ConfigurationManager();
       var  moduleManager = new DefaultModuleManager(typeof(TStartupModule));
        moduleManager.ConfigureServices(services, config);
        AfterConfigureServices(services);
        var containerBuilder = serviceFactory.CreateBuilder(services);
        RootServiceProvider = serviceFactory.CreateServiceProvider(containerBuilder);
        TestServiceScope = RootServiceProvider.CreateScope();
        ServiceProvider = TestServiceScope.ServiceProvider;
        UnitOfWorkManager = ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
        moduleManager.InitializeModulesAsync(ServiceProvider).ConfigureAwait(false).GetAwaiter().GetResult();
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
    protected IServiceProvider ServiceProvider { get; set; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; set; }

    protected T? GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }

    protected T GetRequiredService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }


    protected Task WithUnitOfWorkAsync(Func<Task> func)
    {
        return WithUnitOfWorkAsync(new UnitOfWorkOptions(), func);
    }

    protected async Task WithUnitOfWorkAsync(UnitOfWorkOptions options, Func<Task> action)
    {
        using (var uow = UnitOfWorkManager.Begin(options, true))
        {
            await action();
            await uow.CompleteAsync();
        }
    }

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

   
    [TestInitialize]
    public Task TestInitialize()
    {
        if (AutoCreateUow)
        {
            UnitOfWorkManager.Begin();
        }
       
        return Task.CompletedTask;
    }
    [TestCleanup]

    public Task TestCleanup()
    {
        if (AutoCreateUow)
        {
            var uow = UnitOfWorkManager.Current;
            if (uow != null)
            {
                uow.CompleteAsync();
                uow.Dispose();
            }
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        TestServiceScope.Dispose();
    }
}