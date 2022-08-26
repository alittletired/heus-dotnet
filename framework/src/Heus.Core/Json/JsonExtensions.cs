using System.Text.Json;
using System.Text.Json.Serialization;

namespace Heus.Json;

public static class JsonExtensions
{
    public static void ApplyDefaultSettings(this JsonSerializerOptions options)
    {
        options.PropertyNameCaseInsensitive = true;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.Converters.Add(new JsonEntityIdStringConverter());
    }

}