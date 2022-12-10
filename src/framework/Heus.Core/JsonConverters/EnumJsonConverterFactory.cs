using System.Text.Json;
using System.Text.Json.Serialization;
using Heus.Core.Common;

namespace Heus.Core.JsonConverters;

public class EnumJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableTo<IEnumClass>() &&
               typeToConvert.IsAssignableTo(typeof(EnumClass<>).MakeGenericType(typeToConvert));
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Activator.CreateInstance(GetEnumConverterType(typeToConvert)) as JsonConverter;
    }

    private static Type GetEnumConverterType(Type enumType) => typeof(EnumJsonConverter<>).MakeGenericType(enumType);
}