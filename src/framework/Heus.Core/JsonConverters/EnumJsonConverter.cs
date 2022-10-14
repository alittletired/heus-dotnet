using System.Text.Json;
using System.Text.Json.Serialization;

namespace Heus.Core.JsonConverters;

public class EnumJsonConverter<T>: JsonConverter<T> where T:EnumBase<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number =>EnumBase<T>.FromValue(reader.GetInt32()),
            JsonTokenType.String => EnumBase<T>.FromName(reader.GetString()!),
            _ => throw new InvalidCastException(reader.TokenType.ToString())
        };
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Value);
    }
}