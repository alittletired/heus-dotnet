
using System.Globalization;
using System.Reflection;

namespace System;
//https://github.com/autofac/Autofac/blob/178aa2473a558930b2e0058712f92cfd53b00f85/src/Autofac/TypeExtensions.cs
public static class TypeExtensions
{

    private const BindingFlags DeclaredMemberFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.NonPublic;

    
    /// <summary>
    /// Determines whether an instance of this type can be assigned to
    /// an instance of the <typeparamref name="TTarget"></typeparamref>.
    ///
    /// Internally uses <see cref="Type.IsAssignableFrom"/>.
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam> (as reverse).
    public static bool IsAssignableTo<TTarget>(this Type type)
    {

        return type.IsAssignableTo(typeof(TTarget));
    }

    /// <summary>
    /// Returns an object that represents the specified method declared by the
    /// current type.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>An object that represents the specified method, if found; otherwise, null.</returns>
    public static MethodInfo GetDeclaredMethod(this Type type, string methodName)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(methodName);
        var foundMethod = type.GetMethod(methodName, DeclaredMemberFlags);
        ArgumentNullException.ThrowIfNull(foundMethod);
        return foundMethod;
    }
   
}
