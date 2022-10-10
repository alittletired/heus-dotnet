using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Heus.Core.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Heus.Core.Security;

internal class JwtTokenProvider : ITokenProvider, IScopedDependency
{
    private readonly IOptions<JwtOptions> _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public ClaimsPrincipal CreatePrincipal(ICurrentUser user, TokenType tokenType, bool rememberMe=false)
    {
        var expirationMinutes = _jwtOptions.Value.ExpirationMinutes;
        if (rememberMe)
        {
            expirationMinutes *= 10;
        }

        var unixTimestamp = (long)DateTime.Now.AddMinutes(expirationMinutes).Subtract(DateTime.UnixEpoch).TotalMilliseconds;
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, _jwtOptions.Value.Subject),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()),
            new(SecurityClaimNames.UserId, user.Id?.ToString()!),
            new(SecurityClaimNames.TokenType, tokenType.ToString()),
            new(SecurityClaimNames.Expiration, unixTimestamp.ToString()),

        };

        var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(claimsIdentity);

    }

    public string CreateToken(ClaimsPrincipal principal)
    {
        var unixTimestamp = principal.FindClaimValue<long>(SecurityClaimNames.Expiration);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SignKey));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _jwtOptions.Value.Subject,
            _jwtOptions.Value.Subject,
            principal.Claims,
            expires: DateTime.UnixEpoch.AddMilliseconds(unixTimestamp),
            signingCredentials: signIn);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}