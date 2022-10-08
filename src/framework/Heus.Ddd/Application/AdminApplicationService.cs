using Heus.Ddd.Application.Services;
using Heus.Ddd.Dtos;

namespace Heus.Ddd.Application;

public interface IAdminApplicationService<in TCreateDto, in TUpdateDto, TDto> : IAdminApplicationService
    , IReadOnlyAppService<TDto>
    , ICreateAppService<TCreateDto, TDto>
    , IUpdateAppService<TUpdateDto, TDto>
    , IDeleteAppService
{
  Task<PagedList<TDto>> GetListAsync(DynamicQuery<TDto> input);
}

/// <summary>
/// 管理后台基类
/// </summary>
public abstract class AdminApplicationService: ApplicationService, IAdminApplicationService
{
}