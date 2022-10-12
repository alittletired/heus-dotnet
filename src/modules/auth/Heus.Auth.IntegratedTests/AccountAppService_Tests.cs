using Heus.Auth.Application;
using Heus.Auth.Dtos;
using Heus.Core.Security;
using Heus.IntegratedTests;

namespace Heus.Auth.IntegratedTests;

public class AccountAppServiceTests:IClassFixture<IntegratedTest<Program>>
{
    private readonly IntegratedTest<Program> _factory;
    private  IAccountAdminAppService _accountService=>_factory.GetServiceProxy<IAccountAdminAppService>(AuthConstants.ServiceName);
    public AccountAppServiceTests(IntegratedTest<Program> factory)
    {
        _factory = factory;
        var server = _factory.Server;
    }

    [Theory]
    [InlineData("admin", "1", true)]
    public async Task LoginAsync(string account, string password, bool rememberMe)
    {
        var input = new LoginInput(account, password, rememberMe);
        var result = await _accountService.LoginAsync(input);
        result.AccessToken.ShouldNotBeNullOrEmpty();
      
    }

    // [Theory]
    // [InlineData("admin", "1", true)]
    // public async Task RefreshTokenAsync(string account, string password, bool rememberMe)
    // {
    //     var input = new LoginInput(account, password, rememberMe);
    //     var result = await _accountService.LoginAsync(input);
    //     var newToken = await _accountService.RefreshTokenAsync(result);
    //     newToken.AccessToken.ShouldNotBeNullOrWhiteSpace();
    //     newToken.AccessToken.ShouldNotBe(result.AccessToken);
    // }
   //  [Theory]
   //  [InlineData("")]
   // public Task SendVerifyCodeAsync(string phone)
   //  {
   //      
   //  }
}