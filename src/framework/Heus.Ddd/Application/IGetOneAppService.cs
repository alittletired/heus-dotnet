using Heus.Ddd.Dtos;
using Heus.Ddd.Entities;

namespace Heus.Ddd.Application.Services;
    public interface IGetOneAppService<TEntityDto>
    {
        Task<TEntityDto> GetAsync(long id);
        // Task<PagedList<TEntityDto>> GetListAsync(DynamicQuery<TEntityDto> input);
       
    }

  
