using System.Text.Json;
using System.Text.Json.Serialization;
using Heus.Json;

namespace Heus.Utils;

public static class JsonUtils
{
    public static JsonSerializerOptions Options { get; } = new ();
    public static void ApplyDefaultSettings(this JsonSerializerOptions options)
    {
        options.PropertyNameCaseInsensitive = true;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.Converters.Add(new JsonEntityIdStringConverter());
    }   
    static JsonUtils()
    {
        ApplyDefaultSettings(Options);
    }

    public static string ToJson<T>(T obj) 
    {
       return JsonSerializer.Serialize(obj, Options);
    }

    public static T? FormJson<T>(string? json) {

        if (string.IsNullOrEmpty(json))
        {
            return default!;
        }
        return JsonSerializer.Deserialize<T>(json, Options);
    }
}