using Heus.Auth.Entities;
using Heus.Core.ObjectMapping;
using Heus.Ddd.Entities;

namespace Heus.Auth.Dtos;

[ObjectMapper(typeof(User))]
public partial class UserCreateDto
{
    
}
public partial class UserUpdateDto
{
    public long Id { get; set; }
}