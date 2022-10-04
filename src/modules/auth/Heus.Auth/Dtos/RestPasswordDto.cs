using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos;

public class RestPasswordDto
{
    public RestPasswordDto(EntityId userId, string newPassword)
    {
        UserId = userId;
        NewPassword = newPassword;
    }

    public EntityId UserId { get; set; }
    public string NewPassword{ get; set; }
}