using System.Collections;
using System.Reflection;

namespace Heus.Core;

public class TypeList<TBaseType>: IList<Type>
{
    private readonly List<Type> _typeList = new();
    public void Add<T>() where T : TBaseType
    {
        _typeList.Add(typeof(T));
    }
    public void Remove<T>() where T : TBaseType
    {
        _typeList.Remove(typeof(T));
    }

    public bool Remove(Type item)
    {
        throw new NotImplementedException();
    }

    public int Count => _typeList.Count;
    public bool IsReadOnly => false;

    public void Add(Type item)
    {
        CheckType(item);
        _typeList.Add(item);
    }

    public void Clear()
    {
        _typeList.Clear();
    }

    public bool Contains(Type item)
    {
        return _typeList.Contains(item);
    }

    public void CopyTo(Type[] array, int arrayIndex)
    {
        _typeList.CopyTo(array,arrayIndex);
    }

    private static void CheckType(Type item)
    {
        if (!typeof(TBaseType).GetTypeInfo().IsAssignableFrom(item))
        {
            throw new ArgumentException($"Given type ({item.AssemblyQualifiedName}) should be instance of {typeof(TBaseType).AssemblyQualifiedName} ", nameof(item));
        }
    }

    public IEnumerator<Type> GetEnumerator()
    {
        return _typeList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int IndexOf(Type item) => _typeList.IndexOf(item);


    public void Insert(int index, Type item)
    {
        CheckType(item);
        _typeList.Insert(index,item);
        
    } 
   

    public void RemoveAt(int index)=> _typeList.RemoveAt(index);
  
    public Type this[int index] {
        get => _typeList[index];
        set {
            CheckType(value);
            _typeList[index] = value;
        }
    }
}