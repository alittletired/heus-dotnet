
namespace Heus.Auth.Dtos;

public record LoginResult(long UserId,string NickName, string AccessToken, long Expiration)
{
}