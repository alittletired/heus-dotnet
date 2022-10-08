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
    [AllowAnonymous]
    Task<AuthTokenDto> LoginAsync(LoginInput input);
    Task<AuthTokenDto> RefreshTokenAsync(AuthTokenDto input);

    [AllowAnonymous]
    Task<bool> SendVerifyCodeAsync(string phone);
}

internal class AccountAdminAppService : IAccountAdminAppService
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager _userManager;
    private readonly ITokenProvider _tokenProvider;

    public AccountAdminAppService(IUserRepository userRepository, UserManager userManager,
        ITokenProvider tokenProvider)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _tokenProvider = tokenProvider;
    }

    public async Task<AuthTokenDto> LoginAsync(LoginInput input)
    {
        var user = await _userRepository.FindByAccountAsync(input.Account);
        if (user == null)
        {
            throw EntityNotFoundException.Create(user, nameof(User.Account), input.Account);
        }

        AuthTokenDto authToken = await CreateAuthToken(user, input.RememberMe);
        return authToken;
    }

    private Task<AuthTokenDto> CreateAuthToken(User user, bool rememberMe)
    {
        var (_, err) = _userManager.CheckUserState(user);
        if (err.HasText())
        {
            throw new BusinessException(err);
        }

        var expiration = rememberMe ? 60 * 8 : 60 * 24 * 7;
        var payload = new Dictionary<string, string>
        {
            { nameof(user.Id), user.Id.ToString()! }

        };
        var accessToken = _tokenProvider.CreateToken(payload, expiration);
        var refreshToken = _tokenProvider.CreateToken(payload, 60 * 24 * 60);

        AuthTokenDto authToken = new AuthTokenDto(accessToken, expiration, refreshToken);
        return Task.FromResult<AuthTokenDto>(authToken);
    }

    public Task<AuthTokenDto> RefreshTokenAsync(AuthTokenDto input)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendVerifyCodeAsync(string phone)
    {
        throw new NotImplementedException();
    }
}