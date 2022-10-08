using Heus.Auth.Application;
using Heus.Auth.Dtos;
using Heus.IntegratedTests;
using Shouldly;
using Xunit;

namespace Heus.Auth.IntegratedTests;

public class AccountAppServiceTests:IClassFixture<IntegratedTestHost<Program>>
{
    private readonly IntegratedTestHost<Program> _factory;
    private readonly IAccountAdminAppService _accountService;
    public AccountAppServiceTests(IntegratedTestHost<Program> factory)
    {
        _factory = factory;
        _accountService = _factory.GetServiceProxy<IAccountAdminAppService>(AuthConstants.ServiceName);
    }

    [Theory]
    [InlineData("admin","1",true)]
    public async  Task LoginAsync(string account, string password, bool rememberMe)
    {
        var input = new LoginInput(account, password, rememberMe);
        var result = await _accountService.LoginAsync(input);
        result.AccessToken.ShouldNotBeNullOrEmpty();
        
    }

    [Theory]
    [InlineData("admin", "1", true)]
    public async Task RefreshTokenAsync(string account, string password, bool rememberMe)
    {
        var input = new LoginInput(account, password, rememberMe);
        var result = await _accountService.LoginAsync(input);
        var newToken = await _accountService.RefreshTokenAsync(result);
        newToken.AccessToken.ShouldNotBeNullOrWhiteSpace();
        newToken.AccessToken.ShouldNotBe(result.AccessToken);
    }
   //  [Theory]
   //  [InlineData("")]
   // public Task SendVerifyCodeAsync(string phone)
   //  {
   //      
   //  }
}