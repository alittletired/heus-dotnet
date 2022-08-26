using System.ComponentModel;
using System.Globalization;

namespace System;
public static class ObjectExtensions
{
    /// <summary>
    /// Converts given object to a value type using <see cref="Convert.ChangeType(object,System.Type)"/> method.
    /// </summary>
    /// <param name="obj">Object to be converted</param>
    /// <typeparam name="T">Type of the target object</typeparam>
    /// <returns>Converted object</returns>
    public static T ConvertTo<T>(this object obj)
        where T : struct
    {
        if (typeof(T) == typeof(Guid))
        {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(obj.ToString()!)!;
        }

        return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
    }
}