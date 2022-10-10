namespace Heus.Core.Security;

public class JwtOptions
{
    public const string ConfigurationSection = "Jwt";
    public const string AuthenticationScheme = "Bearer";
    public string SignKey { get; set; } = "JWT:Token:Io:Heus:Framework";
    public string Subject{ get; set; } = "JWTToken";
    public string Issuer { get; set; } = "JWTHeusAuth";
}