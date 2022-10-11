using Heus.Ddd.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Heus.Ddd.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedList<T>> ToPageListAsync<T>(this IQueryable queryable, DynamicQuery<T> dynamicQuery)
    {
        var visiter = new DynamicExpressionVisitor<T>(dynamicQuery);
        var expr = queryable.Expression;
        var t = visiter.VisitStart(expr);
        var pageList = new PagedList<T>();
        var query= queryable.Provider.CreateQuery<T>(expr) ;
        var count =await query.CountAsync();
        if (count > 0)
        {
            pageList.Count = count;
            pageList .Items=await query.Take(dynamicQuery.PageSize).Skip(dynamicQuery.PageSize * dynamicQuery.PageIndex).ToListAsync(); 
        }
        return pageList;
        
    }
}
