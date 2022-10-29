using System.ComponentModel.DataAnnotations;

namespace Heus.Auth.Dtos;

public record LoginInput([Required]string UserName,[Required] string Password,[Required] bool RememberMe)
{
   
}