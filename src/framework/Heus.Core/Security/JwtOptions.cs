namespace Heus.Core.Security;

public class JwtOptions
{
    public const string ConfigurationSection = "Jwt";
    public string SignKey { get; set; } = "JWTToken";
    public string Subject{ get; set; } = "JWTToken";
    public string Issuer { get; set; } = "JWTHeusAuth";
}