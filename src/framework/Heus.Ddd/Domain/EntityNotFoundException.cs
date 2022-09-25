using System.Runtime.Serialization;
using Heus.Core;

namespace Heus.Ddd.Domain;

public class EntityNotFoundException:BusinessException
{
    /// <summary>
    /// Type of the entity.
    /// </summary>
    public Type EntityType { get; set; }
    /// <summary>
    /// Id of the Entity.
    /// </summary>
    public object? Id { get; set; }
    public EntityNotFoundException(Type entityType)
        : this(entityType, null, null)
    {

    }
    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    public EntityNotFoundException(Type entityType, object? id)
        : this(entityType, id, null)
    {

    }

    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    public EntityNotFoundException(Type entityType, object? id, Exception? innerException)
        : base(
            id == null
                ? $"There is no such an entity given id. Entity type: {entityType.FullName}"
                : $"There is no such an entity. Entity type: {entityType.FullName}, id: {id}",
            innerException)
    {
        EntityType = entityType;
        Id = id;
    }
}