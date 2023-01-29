using Heus.AspNetCore.TestBase;
using Heus.Core.Http;
using Heus.Core.Security;
using Heus.Ddd.Application;
using Heus.Ddd.Dtos;
using Heus.Ddd.TestModule.Application;
using Heus.Ddd.TestModule.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Heus.AspNetCore.Tests.Http;


[Route("api/RemoteServiceProxyTest")]

public class HttpRemoteServiceProxyTestController : Controller
{
    private readonly IAddressAppService _addressAppService;

    public HttpRemoteServiceProxyTestController( IServiceProvider serviceProvider )
    {
        _addressAppService = serviceProvider.GetRequiredService<RemoteServiceProxyFactory>().CreateProxy<IAddressAppService>();
    }
    [HttpGet]
    [Route("GetUserAddress")]
    public async Task<PageList<Address>> GetUserAddress(long userId)
    {
        return await _addressAppService.SearchAsync(new DynamicSearch<Address>());
    }
}
public class HttpRemoteServiceProxyTests:AspNetIntegratedTest
{
    private readonly ITokenProvider _tokenProvider;
    public HttpRemoteServiceProxyTests(WebApplicationFactory<AspNetCoreTestModule, Program> factory) : base(factory)
    {
        _tokenProvider = GetRequiredService<ITokenProvider>();
    }

    [Fact]
    public async Task HttpRemoteServiceProxyContributor_Test()
    {
        // var token = _tokenProvider.CreateToken(_currentUser.Principal!);
        // request.Headers.Add("Authorization", "Bearer " + token);
        var request = new HttpRequestMessage();
        request.RequestUri = new Uri( _factory.HttpClient.BaseAddress!, "/api/RemoteServiceProxyTest/GetUserAddress");
        var principal = _tokenProvider.CreatePrincipal(GetCurrentUser());
        var token = _tokenProvider.CreateToken(principal);
        request.Headers.Add("Authorization", "Bearer " + token);
      
        var res=await _factory.HttpClient.SendAsync(request);
      
      
    }

 
}