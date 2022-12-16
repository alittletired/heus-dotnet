using Heus.Auth.Application;
using Heus.Auth.Dtos;

using Heus.TestBase;

namespace Heus.Auth.IntegratedTests;
[TestClass]
public class AccountAppServiceTests : IntegratedTestBase<AuthTestModule>
{

    private readonly IAccountAdminAppService _accountService;
    public AccountAppServiceTests()
    {
        _accountService = GetRequiredService<IAccountAdminAppService>();
    }

    [TestMethod]
    [DataRow("admin", "1", true)]
    public async Task LoginAsync(string account, string password, bool rememberMe)
    {
        var input = new LoginInput(account, password, rememberMe);
        var result = await _accountService.LoginAsync(input);
        result.AccessToken.ShouldNotBeNullOrEmpty();

    }

    // [TestMethod]
    // [DataRow("admin", "1", true)]
    // public async Task RefreshTokenAsync(string account, string password, bool rememberMe)
    // {
    //     var input = new LoginInput(account, password, rememberMe);
    //     var result = await _accountService.LoginAsync(input);
    //     var newToken = await _accountService.RefreshTokenAsync(result);
    //     newToken.AccessToken.ShouldNotBeNullOrWhiteSpace();
    //     newToken.AccessToken.ShouldNotBe(result.AccessToken);
    // }
    //  [TestMethod]
    //  [DataRow("")]
    // public Task SendVerifyCodeAsync(string phone)
    //  {
    //      
    //  }
}