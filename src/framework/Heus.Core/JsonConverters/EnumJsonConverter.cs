using System.Text.Json;
using System.Text.Json.Serialization;
using Heus.Core.Common;

namespace Heus.Core.JsonConverters;

public class EnumJsonConverter<T>: JsonConverter<T> where T:EnumClass<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number =>EnumClass<T>.FromValue(reader.GetInt32()),
            _ =>  EnumClass<T>.FromName(reader.GetString()!)
        };
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}