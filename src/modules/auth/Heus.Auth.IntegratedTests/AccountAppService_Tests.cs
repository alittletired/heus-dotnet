using Heus.Auth.Application;
using Heus.Auth.Dtos;
using Heus.IntegratedTests;

namespace Heus.Auth.IntegratedTests;

[UseUnitOfWork]
[Collection(nameof(IntegratedTestCollection))]
public class AccountAppServiceTests
{
    private readonly IntegratedTest<Program> _factory;
    private readonly  IAccountAdminAppService _accountService;
    public AccountAppServiceTests(IntegratedTest<Program> factory)
    {
        _factory = factory;
        _accountService = _factory.GetServiceProxy<IAccountAdminAppService>(AuthConstants.ServiceName);
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