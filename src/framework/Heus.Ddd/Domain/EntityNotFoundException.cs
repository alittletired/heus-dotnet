

using System.Diagnostics.CodeAnalysis;

namespace Heus.Ddd.Domain;
public class EntityNotFoundException:BusinessException
{
    public static void ThrowIfNull<T>([NotNull] T? argument, string property, object value)
    {
        if (argument != null)
        {
            return;
        }

        Throw(typeof(T), property, value);
    } 
    [DoesNotReturn]
    private static void Throw(Type entityType, string property,object? value)
    {
        throw new EntityNotFoundException(entityType, property, value);
    }

    /// <summary>
    /// Type of the entity.
    /// </summary>
    public Type EntityType { get; }
    
    public string Property { get;  }
    public object? Value { get;  }
    
  
    /// <summary>
    /// Creates a new <see cref="EntityNotFoundException"/> object.
    /// </summary>
    
    public EntityNotFoundException(Type entityType, string property,object? value)
      :base($"entity not found,type:{entityType},{property}:{value}",404)
    {
      
        EntityType = entityType;
        Property = property;
        Value = value;
    }
   
}