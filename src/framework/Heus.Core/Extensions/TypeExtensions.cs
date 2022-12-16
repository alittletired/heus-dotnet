
using System.Globalization;
using System.Reflection;

namespace System;
//https://github.com/autofac/Autofac/blob/178aa2473a558930b2e0058712f92cfd53b00f85/src/Autofac/TypeExtensions.cs
public static class TypeExtensions
{

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
    private const BindingFlags DeclaredConstructorPublicFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
    private const BindingFlags DeclaredConstructorFlags = DeclaredConstructorPublicFlags | BindingFlags.NonPublic;
    private const BindingFlags DeclaredMemberFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.NonPublic;

    /// <summary>
    /// Returns true if this type is in the <paramref name="namespace"/> namespace
    /// or one of its sub-namespaces.
    /// </summary>
    /// <param name="this">The type to test.</param>
    /// <param name="namespace">The namespace to test.</param>
    /// <returns>True if this type is in the <paramref name="namespace"/> namespace
    /// or one of its sub-namespaces; otherwise, false.</returns>
    public static bool IsInNamespace(this Type @this, string @namespace)
    {
        if (@this == null)
        {
            throw new ArgumentNullException(nameof(@this));
        }

        if (@namespace == null)
        {
            throw new ArgumentNullException(nameof(@namespace));
        }

        return @this.Namespace != null &&
            (@this.Namespace == @namespace || @this.Namespace.StartsWith(@namespace + ".", StringComparison.Ordinal));
    }

    /// <summary>
    /// Returns true if this type is in the same namespace as <typeparamref name="T"/>
    /// or one of its sub-namespaces.
    /// </summary>
    /// <param name="this">The type to test.</param>
    /// <returns>True if this type is in the same namespace as <typeparamref name="T"/>
    /// or one of its sub-namespaces; otherwise, false.</returns>
    public static bool IsInNamespaceOf<T>(this Type @this)
    {
        if (@this == null)
        {
            throw new ArgumentNullException(nameof(@this));
        }

        // Namespace is always non-null for concrete type parameters.
        return IsInNamespace(@this, typeof(T).Namespace!);
    }

    

    /// <summary>
    /// Returns an object that represents the specified method declared by the
    /// current type.
    /// </summary>
    /// <param name="this">The type.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>An object that represents the specified method, if found; otherwise, null.</returns>
    public static MethodInfo GetDeclaredMethod(this Type @this, string methodName)
    {
        if (@this is null)
        {
            throw new ArgumentNullException(nameof(@this));
        }

        if (methodName is null)
        {
            throw new ArgumentNullException(nameof(methodName));
        }

        var foundMethod = @this.GetMethod(methodName, DeclaredMemberFlags);
        ArgumentNullException.ThrowIfNull(foundMethod);

        return foundMethod;
    }

    /// <summary>
    /// Returns an object that represents the specified property declared by the
    /// current type.
    /// </summary>
    /// <param name="this">The type.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>An object that represents the specified property, if found; otherwise, null.</returns>
    public static PropertyInfo GetDeclaredProperty(this Type @this, string propertyName)
    {
        if (@this is null)
        {
            throw new ArgumentNullException(nameof(@this));
        }

        if (propertyName is null)
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        var foundProperty = @this.GetProperty(propertyName, DeclaredMemberFlags);
        ArgumentNullException.ThrowIfNull(foundProperty);
        return foundProperty;
    }

    /// <summary>
    /// Returns a collection of instance constructor information that represents the declared constructors
    /// for the type (public and private).
    /// </summary>
    /// <param name="this">The type.</param>
    /// <returns>A collection of instance constructors.</returns>
    public static ConstructorInfo[] GetDeclaredConstructors(this Type @this)
    {
        if (@this is null)
        {
            throw new ArgumentNullException(nameof(@this));
        }

        return @this.GetConstructors(DeclaredConstructorFlags);
    }

    /// <summary>
    /// Returns a collection of instance constructor information that represents the declared constructors
    /// for the type (public only).
    /// </summary>
    /// <param name="this">The type.</param>
    /// <returns>A collection of instance constructors.</returns>
    public static ConstructorInfo[] GetDeclaredPublicConstructors(this Type @this)
    {
        if (@this is null)
        {
            throw new ArgumentNullException(nameof(@this));
        }

        return @this.GetConstructors(DeclaredConstructorPublicFlags);
    }

    /// <summary>
    /// Finds an instance constructor with the matching type parameters.
    /// </summary>
    /// <param name="type">The type being tested.</param>
    /// <param name="constructorParameterTypes">The types of the contractor to find.</param>
    /// <returns>The <see cref="ConstructorInfo"/> is a match is found; otherwise, <c>null</c>.</returns>
    public static ConstructorInfo? GetMatchingConstructor(this Type type, Type[] constructorParameterTypes)
    {
        return type.GetDeclaredConstructors().FirstOrDefault(
            c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(constructorParameterTypes));
    }


}
