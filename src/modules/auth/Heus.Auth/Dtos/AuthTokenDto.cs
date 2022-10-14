using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos;

public record AuthTokenDto(long UserId, string AccessToken, long Expiration)
{
}