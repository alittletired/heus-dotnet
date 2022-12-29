

using Heus.Ddd.Entities;

namespace Heus.Ddd.TestModule.Domain;
public class UserAddress : IEntity
{
    public long Id { get; set; }
    public long AddressId { get; set; }
    public long UserId { get; set; }

}