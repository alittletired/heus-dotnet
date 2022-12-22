
using System.Reflection;

namespace Heus.Core.Utils;


public static class ReflectionUtils
{
    
    /// <summary>
    /// Checks whether <paramref name="givenType"/> implements/inherits <paramref name="genericType"/>.
    /// </summary>
    /// <param name="givenType">Type to check</param>
    /// <param name="genericType">Generic type</param>
    public static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        var givenTypeInfo = givenType.GetTypeInfo();

        if (givenTypeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        foreach (var interfaceType in givenTypeInfo.GetInterfaces())
        {
            if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }

        if (givenTypeInfo.BaseType == null)
        {
            return false;
        }

        return IsAssignableToGenericType(givenTypeInfo.BaseType, genericType);
    }

    //TODO: Summary
    public static List<Type> GetImplementedGenericTypes(Type givenType, Type genericType)
    {
        var result = new List<Type>();
        AddImplementedGenericTypes(result, givenType, genericType);
        return result;
    }

    private static void AddImplementedGenericTypes(List<Type> result, Type givenType, Type genericType)
    {
        var givenTypeInfo = givenType.GetTypeInfo();

        if (givenTypeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            result.TryAdd(givenType);
        }

        foreach (var interfaceType in givenTypeInfo.GetInterfaces())
        {
            if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
            {
                result.TryAdd(interfaceType);
            }
        }

        if (givenTypeInfo.BaseType == null)
        {
            return;
        }

        AddImplementedGenericTypes(result, givenTypeInfo.BaseType, genericType);
    }

  
  
    /// <summary>
    /// Gets value of a property by it's full path from given object
    /// </summary>
    public static object? GetValueByPath(object obj,  string propertyPath)
    {
        var value = obj;
        var currentType = obj.GetType();
        var absolutePropertyPath = propertyPath;
        foreach (var propertyName in absolutePropertyPath.Split('.'))
        {
            var property = currentType.GetProperty(propertyName);
            if (property != null)
            {
                value = property.GetValue(value, null);
                currentType = property.PropertyType;
            }
            else
            {
                value = null;
                break;
            }
        }

        return value;
    }
    public static Action<object, object?> MakeFastPropertySetter(PropertyInfo propertyInfo)
    {
        // SetMethod will be non-null if we're trying to make a setter for it.
        var setMethod = propertyInfo.SetMethod!;

        // DeclaringType will always be set for properties.
        var typeInput = setMethod.DeclaringType!;
        var parameters = setMethod.GetParameters();
        var parameterType = parameters[0].ParameterType;

        // Create a delegate TDeclaringType -> { TDeclaringType.Property = TValue; }
        var propertySetterAsAction = setMethod.CreateDelegate(typeof(Action<,>).MakeGenericType(typeInput, parameterType));
        var callPropertySetterClosedGenericMethod = CallPropertySetterOpenGenericMethod.MakeGenericMethod(typeInput, parameterType);
        var callPropertySetterDelegate = callPropertySetterClosedGenericMethod.CreateDelegate<Action<object, object?>>(propertySetterAsAction);

        return callPropertySetterDelegate;
    }
    private static readonly MethodInfo CallPropertySetterOpenGenericMethod =
       typeof(ReflectionUtils).GetDeclaredMethod(nameof(CallPropertySetter));
    private static void CallPropertySetter<TDeclaringType, TValue>(
       Action<TDeclaringType, TValue> setter, object target, object value)
    {
        setter((TDeclaringType)target, (TValue)value);
    }
}
