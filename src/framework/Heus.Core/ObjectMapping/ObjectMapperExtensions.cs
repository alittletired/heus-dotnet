using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Heus.Core.ObjectMapping;

public static class ObjectMapperExtensions
{
    internal static void AddObjectMapper(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }

    public static void AddObjectMap(this IServiceCollection services, Type source, Type desc)
    {
        TypeAdapterConfig.GlobalSettings.NewConfig(source, desc);
    }

    public static void AddObjectMap<TSource, TDesc>(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.NewConfig<TSource, TDesc>();
    }
}