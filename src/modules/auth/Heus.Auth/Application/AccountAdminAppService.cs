using Heus.Auth.Domain;
using Heus.Auth.Dtos;
using Heus.Auth.Entities;
using Heus.Core.Security;
using Heus.Ddd.Application;
using Heus.Ddd.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Heus.Auth.Application;
public interface IAccountAdminAppService:IAdminApplicationService
{
   
    Task<AuthTokenDto> LoginAsync(LoginInput input);
    Task<AuthTokenDto> RefreshTokenAsync(AuthTokenDto input);

 
    Task<bool> SendVerifyCodeAsync(string phone);
}

internal class AccountAdminAppService :AdminApplicationService,  IAccountAdminAppService
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
    public async Task<AuthTokenDto> LoginAsync(LoginInput input)
    {
        var user = await _userRepository.FindByAccountAsync(input.Account);
        if (user == null)
        {
            throw EntityNotFoundException.Create(user, nameof(User.Account), input.Account);
        }
        var principal = CreateIdentity(user, input.RememberMe);
        _currentPrincipalAccessor.Change(principal);

         AuthTokenDto authToken = await CreateAuthToken(user, input.RememberMe);
        return authToken;
    }
    private ClaimsPrincipal CreateIdentity(User user, bool rememberMe)
    {
        //var identity = new ClaimsIdentity(JwtOptions.AuthenticationScheme);
        //identity.AddClaim(new Claim(nameof(User.Id), user.Id!.ToString()));
        //identity.AddClaim(new Claim(nameof(User.Name), user.Name));
        throw new NotImplementedException();
    }
    private Task<AuthTokenDto> CreateAuthToken(User user, bool rememberMe)
    {
        var (_, err) = _userManager.CheckUserState(user);
        if (err.HasText())
        {
            throw new BusinessException(err);
        }

        //var expiration = rememberMe ? 60 * 8 : 60 * 24 * 7;
        //var payload = new Dictionary<string, string>
        //{
        //    { nameof(user.Id), user.Id.ToString()! }

        //};
        //var accessToken = _tokenProvider.CreateToken(payload, expiration);
        //var refreshToken = _tokenProvider.CreateToken(payload, 60 * 24 * 60);

        //AuthTokenDto authToken = new AuthTokenDto(accessToken, expiration, refreshToken);
        //return Task.FromResult<AuthTokenDto>(authToken);
        throw new NotImplementedException();
    }

    public Task<AuthTokenDto> RefreshTokenAsync(AuthTokenDto input)
    {
        throw new NotImplementedException();
    }
    [AllowAnonymous]
    public Task<bool> SendVerifyCodeAsync(string phone)
    {
        throw new NotImplementedException();
    }
}