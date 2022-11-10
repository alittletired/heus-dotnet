using Heus.Ddd.Domain;
using Heus.Ddd.Query;
using Microsoft.EntityFrameworkCore;

namespace Heus.Ddd.Dtos;

public static class QueryExtensions
{
    public static IQueryable<TDto> CastQueryable<TDto>(this IQueryable queryable)
    {
       
        QueryExpressionVisitor<TDto> visitor = new(queryable);
        return visitor.Translate(null);
    }

    public static async Task<PageList<TDto>> ToPageListAsync<TDto>(this IQueryable queryable, IPageRequest<TDto> queryDto)
    {
       
        QueryExpressionVisitor<TDto> visitor = new(queryable);
       var query= visitor.Translate(queryDto);
        
        var pageList = new PageList<TDto>();
        var count = await query.CountAsync();
        if (count <= 0)
        {
            return pageList;
        }
        pageList.Total = count;
        pageList.Items = await query.Take(queryDto.PageSize)
            .Skip(queryDto.PageSize * (queryDto.PageIndex - 1)).ToListAsync();

        return pageList;

    }

    // public static async Task<IEnumerable<T>> ToDtoList<T>(this IQueryable queryable)
    // {
    //     var query = TranslateQuery(queryable, queryDto);
    // }

    public static async Task<T?> FirstOrDefaultAsync<T>(IQueryable queryable, IPageRequest<T> queryDto) 
    {
        var query = TranslateQuery(queryable, queryDto);
        return await query.FirstOrDefaultAsync();
    }

    public static async Task<T> First<T>(IQueryable queryable, IPageRequest<T> queryDto) 
    {
        var query = TranslateQuery(queryable, queryDto);
        var data= await query.FirstOrDefaultAsync();
        if (data == null)
        {
            throw new EntityNotFoundException(typeof(T));
        }

        return data;
    }

    private static  IQueryable<T> TranslateQuery<T>(this IQueryable queryable, IPageRequest<T> queryDto) 
    {
        var visitor = new QueryExpressionVisitor<T>(queryable);
        return visitor.Translate(queryDto);
    }

}