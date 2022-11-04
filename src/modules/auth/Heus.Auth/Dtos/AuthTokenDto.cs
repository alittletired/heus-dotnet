
using System.ComponentModel.DataAnnotations;

namespace Heus.Auth.Dtos;

public record LoginResult([Required] long UserId,string NickName, string AccessToken, long Expiration)
{
}