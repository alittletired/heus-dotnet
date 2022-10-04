using Heus.Ddd.Dtos;
using Heus.Ddd.Entities;

namespace Heus.Ddd.Application.Services;
    public interface IReadOnlyAppService<TEntityDto>
    {
        Task<TEntityDto> GetAsync(EntityId id);
        // Task<PagedList<TEntityDto>> GetListAsync(DynamicQuery<TEntityDto> input);
       
    }

  
