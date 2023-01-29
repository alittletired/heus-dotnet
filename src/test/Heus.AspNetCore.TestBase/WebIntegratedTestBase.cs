
using Autofac.Core;
using Heus.Core.Common;
using Heus.Core.Http;
using Heus.Core.Security;
using Heus.Core.Utils;
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

    public async Task<T> HttpGetAsync<T>(string url)
    {
        var request = new HttpRequestMessage();
        var tokenProvider = GetRequiredService<ITokenProvider>();
        request.RequestUri = new Uri( _factory.HttpClient.BaseAddress!,url);
        var principal = tokenProvider.CreatePrincipal(GetCurrentUser());
        var token = tokenProvider.CreateToken(principal);
        request.Headers.Add("Authorization", "Bearer " + token);
      
        var res=await _factory.HttpClient.SendAsync(request);
        res.EnsureSuccessStatusCode();
        var content = await res.Content.ReadAsStringAsync();
        var data = JsonUtils.Deserialize<T>(content)!;
        return data;
    }
}
