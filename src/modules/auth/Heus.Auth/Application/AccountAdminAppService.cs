using Heus.Auth.Domain;
using Heus.Auth.Dtos;
using Heus.Auth.Entities;
using Heus.Core.Security;
using Heus.Ddd.Application;
using Heus.Ddd.Domain;
using Microsoft.AspNetCore.Authorization;
namespace Heus.Auth.Application;
public interface IAccountAdminAppService:IAdminApplicationService
{
    Task<LoginResult> LoginAsync(LoginInput input);
    Task<bool> SendVerifyCodeAsync(string phone);
}

internal class AccountAdminAppService : AdminApplicationService, IAccountAdminAppService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager _userManager;
    private readonly ITokenProvider _tokenProvider;
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public AccountAdminAppService(IUserRepository userRepository
        , UserManager userManager,
        ITokenProvider tokenProvider, ICurrentPrincipalAccessor currentPrincipalAccessor)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _tokenProvider = tokenProvider;
        _currentPrincipalAccessor = currentPrincipalAccessor;


    }

    [AllowAnonymous]
    public async Task<LoginResult> LoginAsync(LoginInput input)
    {
        var user = await _userRepository.FindByUserNameAsync(input.UserName);
        if (user == null)
        {
            throw EntityNotFoundException.Create(user, nameof(User.UserName), input.UserName);
        }
        var (_, err) = _userManager.CheckUserState(user);
        if (err.HasText())
        {
            throw new BusinessException(err);
        }
        var principal = _tokenProvider.CreatePrincipal(Mapper.Map<ICurrentUser>(user), TokenType.Admin, input.RememberMe);
        _currentPrincipalAccessor.Change(principal);
        var unixTimestamp = principal.FindClaimValue<long>(SecurityClaimNames.Expiration);
        LoginResult authToken = new(user.Id, user.NickName, _tokenProvider.CreateToken(principal), unixTimestamp);
        return authToken;
    }

    [AllowAnonymous]
    public Task<bool> SendVerifyCodeAsync(string phone)
    {
        throw new NotImplementedException();
    }


}