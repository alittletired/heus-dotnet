namespace Heus.Core.Security;

public class JwtOptions
{
    public const string ConfigurationSection = "Jwt";
    public const string AuthenticationScheme = "Bearer";
    public string SignKey { get; set; } = "Token:Io:Heus:Framework";
    public string Audience { get; set; } = "admin";
    public string Issuer { get; set; } = "https://heus.com/";
    public int ExpirationMinutes { get; set; } = 30;
    
}