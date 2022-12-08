using Heus.Core.JsonConverters;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Heus.Core.Utils;

public static class JsonUtils
{
    public static JsonSerializerOptions DefaultOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.WriteAsString
    };

    static JsonUtils()
    {
        DefaultOptions.Converters.Add(new LongToStringJsonConverter());
        DefaultOptions.Converters.Add(new EnumJsonConverterFactory());
    }

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

    // public static byte[] SerializeToBytes<T>(T obj)
    // {
    //     return Encoding.UTF8.GetBytes(Serialize(obj));
    // }
    // public static T? Deserialize<T>(byte[] bytes)
    // {
    //     return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(bytes));
    // }
    public static string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, DefaultOptions);
    }

    public static T? Deserialize<T>(string? json)
    {
        return (T?)Deserialize(json,typeof(T));
    }

    public static object? Deserialize(string? json, Type type)
    {
        if (string.IsNullOrEmpty(json))
        {
            return default!;
        }
        return JsonSerializer.Deserialize(json, type, DefaultOptions);
    }
}