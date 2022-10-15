using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heus.Settings;
/// <summary>
/// Can be used to store Name/Value (or Key/Value) pairs.
/// </summary>
[Serializable]
public class NameValue<T>
{
    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get;  }

    /// <summary>
    /// Value.
    /// </summary>
    public T Value { get;  }

    public NameValue(string name, T value)
    {
        Name = name;
        Value = value;
    }
}