using Heus.Ddd.Application;
using Heus.Ddd.Domain;
using Microsoft.EntityFrameworkCore;

namespace Heus.Ddd.Dtos;

public static class DynamicQueryExtensions
{
    public static async Task<PagedList<T>> ToPageListAsync<T>(this IQueryable queryable, DynamicQuery<T> dynamicQuery)
    {
        var query = TranslateQuery(queryable, dynamicQuery);
        var pageList = new PagedList<T>();
        var count =await query.CountAsync();
        if (count > 0)
        {
            pageList.Count = count;
            pageList .Items=await query.Take(dynamicQuery.PageSize)
                .Skip(dynamicQuery.PageSize * (dynamicQuery.PageIndex-1)).ToListAsync(); 
        }
        return pageList;
        
    }
   
    public static async Task<TDto?> FirstOrDefaultAsync<TDto>(IQueryable queryable, DynamicQuery<TDto> dynamicQuery)
    {
        var query = TranslateQuery(queryable, dynamicQuery);
        return await query.FirstOrDefaultAsync();
    }

    private static async Task<TDto> First<TDto>(IQueryable queryable, DynamicQuery<TDto> dynamicQuery)
    {
        var query = TranslateQuery(queryable, dynamicQuery);
        var data= await query.FirstOrDefaultAsync();
        if (data == null)
        {
            throw new EntityNotFoundException(typeof(TDto));
        }

        return data;
    }

    private static  IQueryable<T> TranslateQuery<T>(this IQueryable queryable, DynamicQuery<T> dynamicQuery)
    {
        var visitor = new DynamicExpressionVisitor<T>(dynamicQuery);
        var expr = queryable.Expression;
        var newExpr = visitor.VisitStart(expr);
     
        var query= queryable.Provider.CreateQuery<T>(newExpr) ;
        return query;
    }

}