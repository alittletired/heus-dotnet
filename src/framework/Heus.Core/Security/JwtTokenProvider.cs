using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Heus.Core.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Heus.Core.Security;

internal class JwtTokenProvider : ITokenProvider, ISingletonDependency
{
    private readonly IOptions<JwtOptions> _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public ClaimsPrincipal CreatePrincipal(ICurrentUser user, TokenType tokenType, bool rememberMe = false)
    {
        var expirationMinutes = _jwtOptions.Value.ExpirationMinutes;
        if (rememberMe)
        {
            expirationMinutes *= 10;
        }

        var expiration = DateTimeOffset.Now.AddMinutes(expirationMinutes).ToUnixTimeMilliseconds();
        var claims = new List<Claim>
        {

              //令牌颁发者。表示该令牌由谁创建
             new(JwtRegisteredClaimNames.Iss,  _jwtOptions.Value.Issuer),
               //令牌的受众，分大小写的字符串数组
             new(JwtRegisteredClaimNames.Aud, tokenType.ToString()),
                //Subject Identifier，iss提供的终端用户的标识，在iss范围内唯一，最长为255个ASCII个字符，区分大小写
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()!),
              //Expiration time，令牌的过期时间戳。超过此时间的token会作废， 该声明是一个整数，是1970年1月1日以来的秒数
            new(JwtRegisteredClaimNames.Exp, expiration.ToString()),
            //令牌的唯一标识，该声明的值在令牌颁发者创建的每一个令牌中都是唯一的，为了防止冲突，它通常是一个密码学随机值。这个值相当于向结构化令牌中加入了一个攻击者无法获得的随机熵组件，有利于防止令牌猜测攻击和重放攻击。
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //令牌的颁发时间，该声明是一个整数，是1970年1月1日以来的秒数
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()),

        };

        var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(claimsIdentity);

    }

    public string CreateToken(ClaimsPrincipal principal)
    {
        var expiration = principal.FindClaimValue<double>(JwtRegisteredClaimNames.Exp);
        if (expiration == null)
        {
            throw new InvalidDataException("exp claims is missing in principal");
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SignKey));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            principal.FindClaimValue(JwtRegisteredClaimNames.Iss),
            principal.FindClaimValue(JwtRegisteredClaimNames.Aud),
            principal.Claims,
            expires: DateTime.UnixEpoch.AddMilliseconds(expiration.Value),
            signingCredentials: signIn);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}