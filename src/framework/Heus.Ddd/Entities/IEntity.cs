

using System.Numerics;

namespace Heus.Ddd.Entities;

public interface IEntity<TKey> where TKey : IEquatable<TKey>
{
    TKey Id { get; set; }
}
public interface IEntity: IEntity<long>
{
      
}