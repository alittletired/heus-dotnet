namespace Heus.Core.Security;

public class AuthToken
{
    public AuthToken(string accessToken, long expiration)
    {
        AccessToken = accessToken;
        Expiration = expiration;
    }

    public string AccessToken { get;  }
    public long Expiration { get;  }
    public string? RefreshToken { get;  }
}