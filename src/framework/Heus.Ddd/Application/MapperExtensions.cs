


using Heus.Ddd.Entities;
using MapsterMapper;

namespace Heus.Ddd.Application
{
    public static class MapperExtensions
    {
        private readonly static Lazy<IMapper> Mapper = new (() => new Mapper());
        public static T MapTo<T>(this IEntity entity)
        {
            return Mapper.Value.Map<T>(entity);
        }
     
    }
}
