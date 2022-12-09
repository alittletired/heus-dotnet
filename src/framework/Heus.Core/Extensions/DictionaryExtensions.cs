
namespace System.Collections.Generic;

/// <summary>
/// Extension methods for Dictionary.
/// </summary>
public static class DictionaryExtensions
{
  

  
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
        TKey key,
        Func<TKey, TValue> factory)
    {
       
        if (dictionary.TryGetValue(key, out var obj))
        {
            return obj!;
        }

        return dictionary[key] = factory(key);
    }

  

    public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
        IDictionary<TKey, TValue> other)
    {
        foreach (var (key, value) in other)
        {
            dictionary.Add(key, value);
        }

    }

}
