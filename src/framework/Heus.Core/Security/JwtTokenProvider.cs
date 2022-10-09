using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Heus.Core.Security;

internal  class JwtTokenProvider : ITokenProvider
{
    private readonly IOptions<JwtOptions> _jwtOptions;

    public JwtTokenProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }
    public string CreateToken(Dictionary<string,string> payload,int expirationMinutes=30)
    {
        var claims =new List<Claim>(){
            new (JwtRegisteredClaimNames.Sub, _jwtOptions.Value.Subject),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Iat,DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()),
            // new Claim("UserId",userId),
         
        };
        foreach (var pair in payload)
        {
            claims.Add(new Claim(pair.Key,pair.Value) );
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SignKey) );
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _jwtOptions.Value.Subject,
            _jwtOptions.Value.Subject,
            claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: signIn);

       return new JwtSecurityTokenHandler().WriteToken(token);
    }
}