using Heus.Core.ObjectMapping;
namespace Heus.Auth.Dtos;

[MapTo(typeof(User))]
public partial class UserCreateDto
{
    
}
public partial class UserUpdateDto
{
    public long Id { get; set; }
}