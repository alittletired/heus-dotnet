using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Autofac.Core;
using Heus.Core.DependencyInjection;
using Heus.Core.DependencyInjection.Autofac;
using Heus.Core.Security;
using Heus.Core.Uow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Heus.TestBase;

public abstract class IntegratedTestBase: IAsyncLifetime, IDisposable
{
    /// <summary>
    /// 基本上所有集成测试的操作都需要在工作单元里，故默认开启一个工作单元
    /// </summary>
    protected virtual bool AutoCreateUow => true;
    protected virtual bool AutoAuthorize => true;
    protected IServiceProvider ServiceProvider { get; }
    protected IUnitOfWorkManager UnitOfWorkManager => ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
    protected T GetRequiredService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }
    protected IntegratedTestBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
       var moduleManager= serviceProvider.GetRequiredService<IModuleManager>();
        moduleManager.InitializeModulesAsync(serviceProvider).ConfigureAwait(false).GetAwaiter().GetResult();
        // xunit框架每个方法都会从新实例化对象，工作单元作用域在IAsyncLifetime开启并不生效，故只能放在此处
        if (AutoCreateUow)
        {
            UnitOfWorkManager.Begin(ServiceProvider);
        }
        if (AutoAuthorize)
        {
            var principalAccessor = GetRequiredService<ICurrentPrincipalAccessor>();
            var tokenProvider = GetRequiredService<ITokenProvider>();
            var principal = tokenProvider.CreatePrincipal(GetCurrentUser());
            principalAccessor.Change(principal);
        }
       
       
        
    }
    protected virtual ICurrentUser GetCurrentUser() {
        return new MockCurrentUser("integratedTester") { Id = 20221214 };
        }
    


    protected virtual Task BeforeTestAsync()
    {
        return Task.CompletedTask;
    }
    public async Task InitializeAsync()
    {
        //超类的初始化逻辑只能在此处调用，原因是，超类的构造函数中赋值的字段还没执行，导致空引用
        await ServiceProvider.PerformUowTask(BeforeTestAsync);

    }

    public Task DisposeAsync()
    {

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        if (AutoCreateUow)
        {
            UnitOfWorkManager.Current?.CompleteAsync();
            UnitOfWorkManager.Current?.Dispose();
        }
    }
}
public abstract class IntegratedTestBase<TStartupModule> : 
    IntegratedTestBase, IAsyncLifetime, IDisposable where TStartupModule : IModuleInitializer
{

    private static  IServiceProvider CreateServiceProvider() {
        var serviceFactory = new AutofacServiceProviderFactoryFacade();
        var services = new ServiceCollection();
        var config = new ConfigurationManager();
        var moduleManager = new DefaultModuleManager(typeof(TStartupModule));
         moduleManager.ConfigureServices(services, config);
        var containerBuilder = serviceFactory.CreateBuilder(services);
        var serviceProvider = serviceFactory.CreateServiceProvider(containerBuilder);
        
        return serviceProvider;
    }
    protected IntegratedTestBase():base(CreateServiceProvider())
    {
        
    }
   



    protected Task<TResult> WithUnitOfWorkAsync<TResult>(Func<Task<TResult>> func)
    {
        return WithUnitOfWorkAsync(new UnitOfWorkOptions(), func);
    }

    protected async Task<TResult> WithUnitOfWorkAsync<TResult>(UnitOfWorkOptions options, Func<Task<TResult>> func)
    {
        using (var uow = UnitOfWorkManager.Begin(ServiceProvider, options, true))
        {
            var result = await func();
            await uow.CompleteAsync();
            return result;
        }

    }

 
}