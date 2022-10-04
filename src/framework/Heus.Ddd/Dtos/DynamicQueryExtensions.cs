using Heus.Ddd.Application;

namespace Heus.Ddd.Dtos;

public static class DynamicQueryExtensions
{
    public static async Task<PagedList<TDto>> ToPageList<TDto>(IQueryable queryable, DynamicQuery<TDto> dynamicQuery)
    {
        return await Task.FromResult(new PagedList<TDto>(0,new List<TDto>())) ;
    }

}