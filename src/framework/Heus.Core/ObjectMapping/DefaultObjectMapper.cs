using Heus.Core.DependencyInjection;
using MapsterMapper;

namespace Heus.Core.ObjectMapping;

internal class DefaultObjectMapper:IObjectMapper,IScopedDependency
{
    private readonly IMapper _mapper;

    public DefaultObjectMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public  TDestination Map<TDestination>(object source)
    {
        return _mapper.Map<TDestination>(source);
    }

    public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
    {
        return _mapper.Map(source, destination);
    }
}