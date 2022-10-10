using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos;

public record AuthTokenDto(EntityId UserId, string AccessToken, long Expiration)
{
}