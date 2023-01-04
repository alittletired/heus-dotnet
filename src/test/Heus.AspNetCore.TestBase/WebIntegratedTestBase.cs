
using Autofac.Core;
using Heus.Core.Http;
using Heus.TestBase;

namespace Heus.AspNetCore.TestBase;

public abstract class WebIntegratedTestBase<TTestModule,TStartup> : IntegratedTestBase
    ,IClassFixture<WebApplicationFactory<TTestModule,TStartup>> where TStartup : class

{
    protected readonly WebApplicationFactory<TTestModule,TStartup> _factory;
    public WebIntegratedTestBase(WebApplicationFactory<TTestModule,TStartup> factory) : base(factory.Services)
    {
        _factory = factory;
    }
    public TService CreateServiceProxy<TService>(string? remoteServiceName=null) where TService : IRemoteService
    {
        return GetRequiredService<RemoteServiceProxyFactory>().CreateProxy<TService>(remoteServiceName);
    }
}
