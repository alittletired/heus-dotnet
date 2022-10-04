namespace Heus.Auth.Dtos;

public class AuthTokenDto
{
    public AuthTokenDto(string accessToken, long expiration, string refreshToken)
    {
        AccessToken = accessToken;
        Expiration = expiration;
        RefreshToken = refreshToken;
    }

   

    public string AccessToken { get;  }
    public long Expiration { get;  }
    public string RefreshToken { get;  }
   
  
   
}