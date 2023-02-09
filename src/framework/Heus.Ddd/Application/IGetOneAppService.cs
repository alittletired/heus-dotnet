

namespace Heus.Ddd.Application;

public interface IGetOneAppService<TEntityDto>
{
    Task<TEntityDto> GetAsync(long id);
    // Task<PagedList<TEntityDto>> GetListAsync(DynamicQuery<TEntityDto> input);

}

  
