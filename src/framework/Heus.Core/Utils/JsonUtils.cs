using System.Text.Json;
using System.Text.Json.Serialization;

namespace Heus.Core.Utils;

public static class JsonUtils
{
    public static JsonSerializerOptions DefaultOptions { get; } = new()
    {
        PropertyNameCaseInsensitive=true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    public static void ApplyDefaultSettings(this JsonSerializerOptions options)
    {
        options.PropertyNameCaseInsensitive = true;
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        foreach (var converter in DefaultOptions.Converters)
        {
            options.Converters.Add(converter);    
        }
        
    }   


    public static string Stringify<T>(T obj) 
    {
       return JsonSerializer.Serialize(obj, DefaultOptions);
    }

    public static T? Parse<T>(string? json) {

        if (string.IsNullOrEmpty(json))
        {
            return default!;
        }
        return JsonSerializer.Deserialize<T>(json, DefaultOptions);
    }
    public static object? Parse( string? json,Type type) {

        if (string.IsNullOrEmpty(json))
        {
            return default!;
        }
        return JsonSerializer.Deserialize(json,type, DefaultOptions);
    }
}