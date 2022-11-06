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

              //���ư䷢�ߡ���ʾ��������˭����
             new(JwtRegisteredClaimNames.Iss,  _jwtOptions.Value.Issuer),
               //���Ƶ����ڣ��ִ�Сд���ַ�������
             new(JwtRegisteredClaimNames.Aud, tokenType.ToString()),
                //Subject Identifier��iss�ṩ���ն��û��ı�ʶ����iss��Χ��Ψһ���Ϊ255��ASCII���ַ������ִ�Сд
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()!),
              //Expiration time�����ƵĹ���ʱ�����������ʱ���token�����ϣ� ��������һ����������1970��1��1������������
            new(JwtRegisteredClaimNames.Exp, expiration.ToString()),
            //���Ƶ�Ψһ��ʶ����������ֵ�����ư䷢�ߴ�����ÿһ�������ж���Ψһ�ģ�Ϊ�˷�ֹ��ͻ����ͨ����һ������ѧ���ֵ�����ֵ�൱����ṹ�������м�����һ���������޷���õ����������������ڷ�ֹ���Ʋ²⹥�����طŹ�����
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //���Ƶİ䷢ʱ�䣬��������һ����������1970��1��1������������
            new(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()),


        };

        var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(claimsIdentity);

    }

    public string CreateToken(ClaimsPrincipal principal)
    {
        var expiration = principal.FindClaimValue<long>(JwtRegisteredClaimNames.Exp);
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SignKey));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            principal.FindClaimValue(JwtRegisteredClaimNames.Iss),
            principal.FindClaimValue(JwtRegisteredClaimNames.Aud),
            principal.Claims,
            expires: DateTime.UnixEpoch.AddMilliseconds(expiration),
            signingCredentials: signIn);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}