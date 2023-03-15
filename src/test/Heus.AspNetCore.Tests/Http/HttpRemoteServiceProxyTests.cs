using Heus.AspNetCore.Http;
using Heus.AspNetCore.TestBase;
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
        var data= await _addressAppService.SearchAsync(new DynamicSearch<Address>());
        return data;
    }
}
public class HttpRemoteServiceProxyTests:AspNetIntegratedTest
{
    public HttpRemoteServiceProxyTests(WebApplicationFactory<AspNetCoreTestModule, Program> factory) : base(factory)
    {
        
    }

    [Fact]
    public async Task HttpRemoteServiceProxyContributor_Test()
    {
        var data = await HttpGetAsync<PageList<Address>>("/api/RemoteServiceProxyTest/GetUserAddress");
        data.Items.ShouldNotBeEmpty();
      
    }

 
}