
using Autofac.Core;
using Heus.Core.Http;
using Heus.TestBase;

namespace Heus.AspNetCore.TestBase;

public abstract class WebIntegratedTestBase<T> : IntegratedTestBase, IClassFixture<T> where T : class, IWebApplicationFactory
{
    protected readonly T _factory;
    public WebIntegratedTestBase(T factory) : base(factory.Services)
    {
        _factory = factory;
    }
    public TService CreateServiceProxy<TService>(string? remoteServiceName=null) where TService : IRemoteService
    {
        return GetRequiredService<RemoteServiceProxyFactory>().CreateProxy<TService>(remoteServiceName);
    }
}
