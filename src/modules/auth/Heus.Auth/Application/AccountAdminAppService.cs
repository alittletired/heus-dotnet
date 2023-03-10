
using System.IdentityModel.Tokens.Jwt;
using Heus.Core.Security;
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
        var user = await _userRepository.FindByNameAsync(input.UserName);
        EntityNotFoundException.ThrowIfNull(user,nameof(User.Name) , input.UserName);
        var (_, err) = _userManager.CheckUserState(user);
        if (err.HasText())
        {
            throw new BusinessException(err);
        }

        var principal = _tokenProvider.CreatePrincipal(Mapper.Map<ICurrentUser>(user),  input.RememberMe);
        _currentPrincipalAccessor.Change(principal);
        var unixTimestamp = principal.FindClaimValue<long>(JwtRegisteredClaimNames.Exp);
        LoginResult authToken = new(user.Id, user.NickName, _tokenProvider.CreateToken(principal), unixTimestamp!.Value);
        return authToken;
    }

    [AllowAnonymous]
    public Task<bool> SendVerifyCodeAsync(string phone)
    {
        throw new NotImplementedException();
    }


}