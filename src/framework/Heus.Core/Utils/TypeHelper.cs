using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace Heus.Core.Utils
{
    public static class TypeHelper
    {
        private static readonly HashSet<Type> FloatingTypes = new()
        {
            typeof(float),
            typeof(double),
            typeof(decimal)
        };

        private static readonly HashSet<Type> NonNullablePrimitiveTypes = new()
        {
            typeof(byte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(sbyte),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(bool),
            typeof(float),
            typeof(decimal),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
            typeof(Guid)
        };
        public static bool IsRecordType(Type type)
        {
            return type.GetMethods().Any(m => m.Name == "<Clone>$");
        }
        public static bool IsNonNullablePrimitiveType(Type type)
        {
            return NonNullablePrimitiveTypes.Contains(type);
        }

        public static bool IsFunc(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            var type = obj.GetType();
            if (!type.GetTypeInfo().IsGenericType)
            {
                return false;
            }

            return type.GetGenericTypeDefinition() == typeof(Func<>);
        }

        public static bool IsFunc<TReturn>(object? obj)
        {
            return obj != null && obj.GetType() == typeof(Func<TReturn>);
        }
        private static NullabilityInfoContext _nullabilityContext = new NullabilityInfoContext();
        public static bool IsNullable(PropertyInfo property)
        {
            var nullabilityInfo = _nullabilityContext.Create(property);
            return nullabilityInfo.WriteState is NullabilityState.Nullable;
        }
    

      


        public static bool IsNullable(Type type)
        {

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        }

        public static Type GetFirstGenericArgumentIfNullable(this Type t)
        {
            if (t.GetGenericArguments().Length > 0 && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return t.GetGenericArguments().First();
            }

            return t;
        }

        public static bool IsEnumerable(Type type, out Type? itemType)
        {
       

            var enumerableTypes = ReflectionHelper.GetImplementedGenericTypes(type, typeof(IEnumerable<>));
            if (enumerableTypes.Count == 1)
            {
                itemType = enumerableTypes[0].GenericTypeArguments[0];
                return true;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                itemType = typeof(object);
                return true;
            }

            itemType = null;
            return false;
        }

        public static bool IsDictionary(Type type, out Type? keyType, out Type? valueType)
        {
            var dictionaryTypes = ReflectionHelper
                .GetImplementedGenericTypes(
                    type,
                    typeof(IDictionary<,>)
                );

            if (dictionaryTypes.Count == 1)
            {
                keyType = dictionaryTypes[0].GenericTypeArguments[0];
                valueType = dictionaryTypes[0].GenericTypeArguments[1];
                return true;
            }

            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                keyType = typeof(object);
                valueType = typeof(object);
                return true;
            }

            keyType = null;
            valueType = null;

            return false;
        }

        private static bool IsPrimitiveExtendedInternal(Type type, bool includeEnums)
        {
            if (type.IsPrimitive)
            {
                return true;
            }

            if (includeEnums && type.IsEnum)
            {
                return true;
            }

            return type == typeof(string) ||
                   type == typeof(decimal) ||
                   type == typeof(DateTime) ||
                   type == typeof(DateTimeOffset) ||
                   type == typeof(TimeSpan) ||
                   type == typeof(Guid);
        }



        public static object? GetDefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }


        public static string GetFullNameHandlingNullableAndGenerics( Type type)
        {

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return type.GenericTypeArguments[0].FullName + "?";
            }

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                return $"{genericType.FullName!.Substring(0,genericType.FullName.IndexOf('`'))}<{type.GenericTypeArguments.Select(GetFullNameHandlingNullableAndGenerics).JoinAsString(",")}>";
            }

            return type.FullName ?? type.Name;
        }

        public static string GetSimplifiedName(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return GetSimplifiedName(type.GenericTypeArguments[0]) + "?";
            }

            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                return $"{genericType.Name.Substring(0,genericType.Name.IndexOf('`'))}<{type.GenericTypeArguments.Select(GetSimplifiedName).JoinAsString(",")}>";
            }

            if (type == typeof(string))
            {
                return "string";
            }
            else if (type == typeof(int))
            {
                return "number";
            }
            else if (type == typeof(long))
            {
                return "number";
            }
            else if (type == typeof(bool))
            {
                return "boolean";
            }
            else if (type == typeof(char))
            {
                return "string";
            }
            else if (type == typeof(double))
            {
                return "number";
            }
            else if (type == typeof(float))
            {
                return "number";
            }
            else if (type == typeof(decimal))
            {
                return "number";
            }
            else if (type == typeof(DateTime))
            {
                return "string";
            }
            else if (type == typeof(DateTimeOffset))
            {
                return "string";
            }
            else if (type == typeof(TimeSpan))
            {
                return "string";
            }
            else if (type == typeof(Guid))
            {
                return "string";
            }
            else if (type == typeof(byte))
            {
                return "number";
            }
            else if (type == typeof(sbyte))
            {
                return "number";
            }
            else if (type == typeof(short))
            {
                return "number";
            }
            else if (type == typeof(ushort))
            {
                return "number";
            }
            else if (type == typeof(uint))
            {
                return "number";
            }
            else if (type == typeof(ulong))
            {
                return "number";
            }
            else if (type == typeof(IntPtr))
            {
                return "number";
            }
            else if (type == typeof(UIntPtr))
            {
                return "number";
            }
            else if (type == typeof(object))
            {
                return "object";
            }

            return  type.Name;
        }

        public static object? ConvertFromString<TTargetType>(string value)
        {
            return ConvertFromString(typeof(TTargetType), value);
        }

        public static object? ConvertFromString(Type targetType, string? value)
        {
            if (value == null)
            {
                return null;
            }

            var converter = TypeDescriptor.GetConverter(targetType);

    

            return converter.ConvertFromString(value);
        }

      

        public static object? ConvertFrom<TTargetType>(object value)
        {
            return ConvertFrom(typeof(TTargetType), value);
        }

        public static object? ConvertFrom(Type targetType, object value)
        {
            return TypeDescriptor
                .GetConverter(targetType)?
                .ConvertFrom(value);
        }

   

        public static bool IsDefaultValue(object? obj)
        {
            if (obj == null)
            {
                return true;
            }

            return obj.Equals(GetDefaultValue(obj.GetType()));
        }
        public static List<TFieldType> GetFields<TFieldType>(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(p => type.IsAssignableFrom(p.FieldType))
                .Select(pi => (TFieldType)pi.GetValue(null)!)
                .ToList();
        }
    }
}
