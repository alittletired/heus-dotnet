using System.ComponentModel.DataAnnotations;
using Heus.Core.ObjectMapping;
namespace Heus.Auth.Dtos;

[MapTo(typeof(User))]
public record class UserCreateDto: UserBaseDto
{
    public required string InitialPassword { get; init; } = null!;
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