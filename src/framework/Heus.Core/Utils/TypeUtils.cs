using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Heus.Core.Utils
{
    public static class TypeUtils
    {
        private readonly static Dictionary<Type, string> _simplifiedNames = new() { 
            {typeof(string), "string" },
            {typeof(int),"number"},
            {typeof(long),"number"},
            {typeof(bool), "boolean" },
            {typeof(char), "string" },
            {typeof(double), "number" },
            {typeof(float),"number"},
            {typeof(decimal), "number" },
            {typeof(DateTime), "string" },
            { typeof(DateTimeOffset),"string" },
            { typeof(TimeSpan),"string"},
            { typeof(Guid),"string"}
            ,{ typeof(byte),"number"},
            { typeof(sbyte),"number"},
            { typeof(short),"number"},
            { typeof(ushort),"number"},
            { typeof(uint),"number"},
            { typeof(ulong),"number"},
            { typeof(object),"object"}
    };
        public static bool IsRecordType(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);
            return type.GetMethods().Any(m => m.Name == "<Clone>$");
        }
   
        private readonly static NullabilityInfoContext NullabilityContext = new ();
        public static bool IsNullable(PropertyInfo property)
        {
            var nullabilityInfo = NullabilityContext.Create(property);
            return nullabilityInfo.WriteState is NullabilityState.Nullable;
        }

        public static bool IsAnonymousType(Type type)
        {
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
          && type.IsGenericType && type.Name.Contains("AnonymousType")
          && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
          && type.Attributes.HasFlag(TypeAttributes.NotPublic);
        }

        public static bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static Type? GetEnumerableItemType(Type type)
        {
            var enumerableTypes = ReflectionUtils.GetImplementedGenericTypes(type, typeof(IEnumerable<>));
            if (enumerableTypes.Count == 1)
            {
                return enumerableTypes[0].GenericTypeArguments[0];
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
               return typeof(object);
            }
            return null;
          
        }

        public static object? GetDefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
        public static bool IsDefaultValue(object? obj)
        {
            if (obj == null)
            {
                return true;
            }

            return obj.Equals(GetDefaultValue(obj.GetType()));
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
            if (_simplifiedNames.TryGetValue(type, out var simplifiedName))
            {
                return simplifiedName;
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
        
        public static List<TFieldType> GetFields<TFieldType>(Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(p => type.IsAssignableFrom(p.FieldType))
                .Select(pi => (TFieldType)pi.GetValue(null)!)
                .ToList();
        }
    }
}
