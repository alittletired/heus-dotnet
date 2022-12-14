using System.Security.Cryptography;
using System.Text;

namespace System;
/// <summary>
/// Extension methods for String class.
/// </summary>
public static class StringExtensions
{

    public static bool HasText(this string? obj)
    {
        return !string.IsNullOrEmpty(obj);
    }
    /// <summary>
    /// Indicates whether this string is null or an System.String.Empty string.
    /// </summary>
    public static bool IsNullOrEmpty(this string? str)
    {
        return string.IsNullOrEmpty(str);
    }

    /// <summary>
    /// indicates whether this string is null, empty, or consists only of white-space characters.
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }


    /// <summary>
    /// Converts line endings in the string to <see cref="Environment.NewLine"/>.
    /// </summary>
    public static string NormalizeLineEndings(string str)
    {
        return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
    }
    /// <summary>
    /// Converts given string to a byte array using the given <paramref name="encoding"/>
    /// </summary>
    public static byte[] GetBytes(this string str, Encoding? encoding =null)
    {
        if (encoding == null)
        {
            encoding = Encoding.UTF8;
        }
        return encoding.GetBytes(str);
    }

    public static string JoinAsString(this IEnumerable<string> strList, string separator)
    {
        return string.Join(separator, strList);
    }
   
}
