using Heus.Ddd.Application.Services;
using Heus.Ddd.Dtos;

namespace Heus.Ddd.Application;

public interface IManageService<in TCreateDto, in TUpdateDto, TDto> :IManageService
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
public abstract class ManagementService:ServiceBase,IManageService
{
  public override IServiceProvider ServiceProvider { get; set; } = null!;
}