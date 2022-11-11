using System.ComponentModel.DataAnnotations;
using Heus.Core.ObjectMapping;
namespace Heus.Auth.Dtos;

[MapTo(typeof(User))]
public record class UserCreateDto: UserBaseDto
{
    public string InitialPassword { get; init; } = null!;
}
public record UserUpdateDto : UserBaseDto
{
    public long Id { get; init; }
}
public abstract record UserBaseDto() {
    [Required]
    public string Name { get; init; } =null!;
    [Required]
    public string Phone { get; init; } = null!;
    [Required]
    public string NickName { get; init; } = null!;
}