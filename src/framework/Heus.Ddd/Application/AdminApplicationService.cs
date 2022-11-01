using Heus.Core.DependencyInjection;
using Heus.Ddd.Application.Services;
using Heus.Ddd.Dtos;

namespace Heus.Ddd.Application;

public interface IAdminApplicationService<in TCreateDto, in TUpdateDto, TDto> : IAdminApplicationService
    , IGetOneAppService<TDto>
    , ICreateAppService<TCreateDto, TDto>
    , IUpdateAppService<TUpdateDto, TDto>
    , IDeleteAppService
{
  Task<PageList<TDto>> SearchAsync(DynamicSearch<TDto> input);
}

/// <summary>
/// 管理后台基类
/// </summary>
public abstract class AdminApplicationService: ApplicationService, IAdminApplicationService
{
}