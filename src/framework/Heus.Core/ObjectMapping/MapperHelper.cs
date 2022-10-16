using Mapster;

namespace Heus.Core.ObjectMapping;

internal static class MapperHelper
{
    public static void AddObjectMap(Type source, Type desc, MapType mapType)
    {
    
        switch (mapType)
        {
            case MapType.MapFrom:
                TypeAdapterConfig.GlobalSettings.NewConfig(desc, source);
                break;
            case MapType.MapTo:
                TypeAdapterConfig.GlobalSettings.NewConfig(source, desc);
                break;
            case MapType.TwoWay:
                TypeAdapterConfig.GlobalSettings.NewConfig(desc, source);
                TypeAdapterConfig.GlobalSettings.NewConfig(source, desc);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }



    }

    public static void AddObjectMap<TSource, TDesc>()
    {
        TypeAdapterConfig.GlobalSettings.NewConfig<TSource, TDesc>().TwoWays();
    }
}