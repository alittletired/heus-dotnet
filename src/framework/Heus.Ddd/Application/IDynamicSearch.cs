
using Heus.Ddd.Dtos;

namespace Heus.Ddd.Application;
public interface IDynamicSearch<TDto>: IAppService
{
    Task<PageList<TDto>> SearchAsync(DynamicSearch<TDto> input);
}
