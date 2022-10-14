using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos;

public class RestPasswordDto
{
    public RestPasswordDto(long userId, string newPassword)
    {
        UserId = userId;
        NewPassword = newPassword;
    }

    public long UserId { get; set; }
    public string NewPassword{ get; set; }
}