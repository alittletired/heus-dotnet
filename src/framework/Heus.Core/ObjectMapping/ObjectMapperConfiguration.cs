using AutoMapper;


namespace Heus.Core.ObjectMapping;

internal class ObjectMapperConfiguration
{
    private static MapperConfiguration _instance = null!;
    public static MapperConfiguration Instance => _instance;
}
