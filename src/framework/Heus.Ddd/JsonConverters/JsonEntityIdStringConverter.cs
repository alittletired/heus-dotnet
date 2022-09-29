using System.Text.Json;
using System.Text.Json.Serialization;
using Heus.Core.Data;
using Heus.Ddd.Entities;

namespace Heus.Ddd.JsonConverters;

internal class JsonEntityIdStringConverter:JsonConverter<EntityId>
{
    public override EntityId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
       return EntityId.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, EntityId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}