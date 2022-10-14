using System.Text.Json;
using System.Text.Json.Serialization;

namespace Heus.Core.JsonConverters;

public class EnumJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableTo(typeof(EnumBase<>).MakeGenericType(typeToConvert));
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Activator.CreateInstance(GetEnumConverterType(typeToConvert)) as JsonConverter;
    }

    private static Type GetEnumConverterType(Type enumType) => typeof(EnumJsonConverter<>).MakeGenericType(enumType);
}