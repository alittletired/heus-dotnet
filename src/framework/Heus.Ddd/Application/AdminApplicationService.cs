using Heus.Ddd.Application.Services;
using Heus.Ddd.Dtos;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;

namespace Heus.Ddd.Application;

public interface IAdminApplicationService<TEntity>: IAdminApplicationService<TEntity, TEntity,  TEntity, TEntity> where TEntity : class, IEntity  { }
public interface IAdminApplicationService<TEntity, TDto, in TCreateDto, in TUpdateDto> : IAdminApplicationService
    , IGetOneAppService<TDto>
    , ICreateAppService<TCreateDto, TDto>
    , IUpdateAppService<TUpdateDto, TDto>
    , IDeleteAppService where TEntity : class, IEntity
{
  Task<PageList<TDto>> SearchAsync(DynamicSearch<TDto> input);
}


/// <summary>
/// 管理后台基类
/// </summary>
public abstract class AdminApplicationService: ApplicationService, IAdminApplicationService
{
}
public abstract class AdminApplicationService<TEntity>: AdminApplicationService<TEntity, TEntity, TEntity, TEntity> where TEntity : class, IEntity { }
public abstract class AdminApplicationService<TEntity, TDto, TCreateDto, TUpdateDto> : ApplicationService,
    IAdminApplicationService<TEntity, TDto, TCreateDto, TUpdateDto> where TEntity : class, IEntity
{
    protected IRepository<TEntity> Repository => GetRequiredService<IRepository<TEntity>>();

    public virtual async Task DeleteAsync(long id)
    {
        await Repository.DeleteByIdAsync(id);
    }

    public virtual async Task<TDto> GetAsync(long id)
    {
        var entity = await Repository.GetByIdAsync(id);
        return Mapper.Map<TDto>(entity);
    }

    public virtual async Task<PageList<TDto>> SearchAsync(DynamicSearch<TDto> input)
    {
        var query = Repository.Query;
        return await query.ToPageListAsync(input);
    }

    public virtual async Task<TDto> UpdateAsync(TUpdateDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(updateDto);
        var entity = Mapper.Map<TEntity>(updateDto);
        await Repository.UpdateAsync(entity);
        return Mapper.Map<TDto>(entity);

    }

    public virtual async Task<TDto> CreateAsync(TCreateDto createDto)
    {
        ArgumentNullException.ThrowIfNull(createDto);
        var entity = Mapper.Map<TEntity>(createDto);
        await Repository.InsertAsync(entity);
        return Mapper.Map<TDto>(entity); ;
    }
}