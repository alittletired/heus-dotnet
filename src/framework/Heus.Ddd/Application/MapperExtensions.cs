


using Heus.Ddd.Entities;
using MapsterMapper;

namespace Heus.Ddd.Application
{
    public static class MapperExtensions
    {
        private static readonly Lazy<IMapper> _mapper = new Lazy<IMapper>(() => new Mapper());
        public static T MapTo<T>(this IEntity entity)
        {
            return _mapper.Value.Map<T>(entity);
        }
        public static T? MapToOrNull<T>(this IEntity? entity) where T : class
        {
            if (entity == null)
            {
                return null;
            }
            return _mapper.Value.Map<T>(entity);
        }
    }
}
