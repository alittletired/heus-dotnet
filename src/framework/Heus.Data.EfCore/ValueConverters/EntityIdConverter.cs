using Heus.Ddd.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Heus.Ddd.Data.ValueConversion;

public class EntityIdConverter: ValueConverter<EntityId, string>
{
    public EntityIdConverter()
        : base(
            v => v.ToString(),
            v => new EntityId(v))
    {
    }
}