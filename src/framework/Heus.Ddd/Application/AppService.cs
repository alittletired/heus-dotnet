using System.Linq.Expressions;
using Heus.Core.DependencyInjection;
using Heus.Core.ObjectMapping;
using Heus.Core.Security;
using Heus.Ddd.Application.Services;
using Heus.Ddd.Domain;
using Heus.Ddd.Dtos;
using Heus.Ddd.Entities;
using Heus.Ddd.Repositories;
using MapsterMapper;
using Microsoft.Extensions.Logging;
namespace Heus.Ddd.Application;

public abstract class AppService : IAppService,  IScopedDependency
{
    [Autowired]
    protected ICachedServiceProvider? ServiceProvider { get;  set; }

    protected IObjectMapper Mapper => GetRequiredService<IObjectMapper>();
    protected ILogger Logger => GetRequiredService<ILoggerFactory>().CreateLogger(GetType());

    protected T GetRequiredService<T>() where T : class
    {
        ArgumentNullException.ThrowIfNull(ServiceProvider);
        return ServiceProvider.GetRequiredService<T>();
    }

    protected ICurrentUser CurrentUser => GetRequiredService<ICurrentUser>();

  
}


public interface IAppService<TEntity> : IAppService<TEntity, TEntity, TEntity, TEntity>
    where TEntity : class, IEntity
{ }
public interface IAppService<TEntity, TDto> : IAppService<TEntity, TDto, TDto, TDto>
    where TEntity : class, IEntity
{ }
public interface IAppService<TEntity, TDto, in TCreateDto, in TUpdateDto> : IAppService
    , IGetOneAppService<TDto>
    , ICreateAppService<TCreateDto, TDto>
    , IUpdateAppService<TUpdateDto, TDto>
    , IDeleteAppService where TEntity : class, IEntity
{
   
}



public abstract class AppService<TEntity> : AppService<TEntity, TEntity, TEntity, TEntity>
    where TEntity : class, IEntity
{ }
public abstract class AppService<TEntity, TDto> : AppService<TEntity, TDto, TDto, TDto>
    where TEntity : class, IEntity
{ }

public abstract class AppService<TEntity, TDto, TCreateDto, TUpdateDto> : AppService,
    IAppService<TEntity, TDto, TCreateDto, TUpdateDto> where TEntity : class, IEntity
{
    protected IRepository<TEntity> Repository => GetRequiredService<IRepository<TEntity>>();

    public virtual async Task<long> DeleteAsync(long id)
    {
        await Repository.DeleteByIdAsync(id);
        return id;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual IQueryable<TDto> GetQuery(Expression<Func<TEntity, bool>>? filter = null)
    {
        var query = Repository.Query;
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query.CastQueryable<TEntity, TDto>();
    }

    public virtual async Task<TDto> GetAsync(long id)
    {
        var dto = await GetQuery(s => s.Id == id).FirstOrDefaultAsync();
        EntityNotFoundException.ThrowIfNull(dto, nameof(IEntity.Id), id);

        return dto;
    }

    //public virtual async Task<PageList<TDto>> SearchAsync(DynamicSearch<TDto> input)
    //{
    //    var query = GetQuery();
    //    return await query.ToPageListAsync(input);
    //}

    public virtual async Task<TDto> UpdateAsync(TUpdateDto updateDto)
    {
        ArgumentNullException.ThrowIfNull(updateDto);
        var idProp = typeof(TUpdateDto).GetProperty("Id");

        if (idProp == null)
        {
            throw new InvalidOperationException($"{typeof(TUpdateDto)}±ÿ–Î”–Id Ù–‘");
        }
        var idObj = idProp.GetValue(updateDto);
        if (idObj == null)
        {
            ArgumentNullException.ThrowIfNull(idObj);
        }
        var entity = await Repository.GetByIdAsync((long)idObj);
        Mapper.Map(updateDto, entity);
        await Repository.UpdateAsync(entity);
        return await MapToDto(entity);

    }

    private Task<TDto> MapToDto(TEntity entity)
    {
        if (entity is TDto dto)
        {
            return Task.FromResult(dto);
        }

        dto = Mapper.Map<TDto>(entity);
        return Task.FromResult(dto);

    }

    public virtual async Task<TDto> CreateAsync(TCreateDto createDto)
    {
        ArgumentNullException.ThrowIfNull(createDto);
        var entity = Mapper.Map<TEntity>(createDto);
        await Repository.InsertAsync(entity);

        return await MapToDto(entity);

    }
}