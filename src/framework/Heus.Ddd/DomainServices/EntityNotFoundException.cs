using System.Runtime.Serialization;
using Heus.Core;
using Heus.Ddd.Entities;

namespace Heus.Ddd.Domain;
public class EntityNotFoundException:BusinessException
{
    public static EntityNotFoundException Create<TEntity>(TEntity? entity, string findBy, string value)
    {
        return new EntityNotFoundException(typeof(TEntity), findBy, value);
    }
    /// <summary>
    /// Type of the entity.
    /// </summary>
    public Type EntityType { get; set; }
    
    public string FindBy { get; set; }
    public object? Value { get; set; }
    public EntityNotFoundException(Type entityType)
        : this(entityType, "id", null)
    {

    }
  
    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    public EntityNotFoundException(Type entityType, EntityId id)
        : this(entityType,"id", id)
    {

    }
    public EntityNotFoundException(Type entityType, string findBy,object? value)
      :base($"entity not found,type:{entityType},{findBy}:{value}")
    {
        EntityType = entityType;
        FindBy = findBy;
        Value = value;
    }
   
}