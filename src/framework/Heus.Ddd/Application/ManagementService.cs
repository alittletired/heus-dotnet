using Heus.Ddd.Application.Services;
using Heus.Ddd.Dtos;

namespace Heus.Ddd.Application;

public interface IManagementService : IRemoteService
{
    
}

public interface IManagementService<in TCreateDto, in TUpdateDto, TDto> :IManagementService
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
public abstract class ManagementService:ServiceBase,IManagementService
{
  public override IServiceProvider ServiceProvider { get; set; } = null!;
}