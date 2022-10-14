using System.Text.Json;
using System.Text.Json.Serialization;

namespace Heus.Core.JsonConverters
{
    internal class LongToStringJsonConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.Number when reader.TryGetInt64(out var l) => l,
                JsonTokenType.Number => (long)reader.GetDouble(),
                JsonTokenType.String => long.Parse(reader.GetString()!),
                _ => throw new InvalidCastException(reader.TokenType.ToString())
            };
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
