using System.Text.Json;
using System.Text.Json.Serialization;

namespace Heus.Core.JsonConverters
{
    internal class LongToStringJsonConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch {
                JsonTokenType.Number => reader.GetInt64(),
                _ => long.Parse(reader.GetString()!)
            };
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
