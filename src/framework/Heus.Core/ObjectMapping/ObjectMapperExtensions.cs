using Mapster;
using MapsterMapper;

namespace Heus.Core.ObjectMapping;

public static class ObjectMapperExtensions
{
    internal static void AddObjectMapper(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }

 
}