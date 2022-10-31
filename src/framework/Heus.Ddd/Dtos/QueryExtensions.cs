using Heus.Ddd.Domain;
using Heus.Ddd.Qeury;
using Microsoft.EntityFrameworkCore;

namespace Heus.Ddd.Dtos;

public static class QueryExtensions
{
    public static async Task<PagedList<T>> ToPageListAsync<T>(this IQueryable queryable, IQueryDto<T> queryDto)
    {
        var query = TranslateQuery(queryable, queryDto);
        var pageList = new PagedList<T>();
        var count = await query.CountAsync();
        if (count > 0)
        {
            pageList.Total = count;
            pageList.Items = await query.Take(queryDto.PageSize)
                .Skip(queryDto.PageSize * (queryDto.PageIndex - 1)).ToListAsync();
        }

        return pageList;

    }

    public static async Task<T?> FirstOrDefaultAsync<T>(IQueryable queryable, IQueryDto<T> queryDto) 
    {
        var query = TranslateQuery(queryable, queryDto);
        return await query.FirstOrDefaultAsync();
    }

    public static async Task<T> First<T>(IQueryable queryable, IQueryDto<T> queryDto) 
    {
        var query = TranslateQuery(queryable, queryDto);
        var data= await query.FirstOrDefaultAsync();
        if (data == null)
        {
            throw new EntityNotFoundException(typeof(T));
        }

        return data;
    }

    private static  IQueryable<T> TranslateQuery<T>(this IQueryable queryable, IQueryDto<T> queryDto) 
    {
        var visitor = new QueryExpressionVisitor<T>(queryDto);
        var expr = queryable.Expression;
        var newExpr = visitor.VisitStart(expr);
     
        var query= queryable.Provider.CreateQuery<T>(newExpr) ;
        return query;
    }

}