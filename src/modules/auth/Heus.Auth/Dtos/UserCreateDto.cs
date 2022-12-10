using System.ComponentModel.DataAnnotations;
using Heus.Core.ObjectMapping;
namespace Heus.Auth.Dtos;

[ObjectMapping(typeof(User), MapType.MapTo)]
public record class UserCreateDto: UserBaseDto
{
    public required string PlaintextPassword { get; init; } 
}
public record UserUpdateDto : UserBaseDto
{
    public required long Id { get; init; }
}
public abstract record UserBaseDto() {
    
    public required string Name { get; init; } 
  
    public required string Phone { get; init; } = null!;
    
    public required string NickName { get; init; } = null!;
}