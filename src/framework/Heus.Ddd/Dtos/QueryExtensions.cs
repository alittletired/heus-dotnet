using Heus.Ddd.Query;
namespace Heus.Ddd.Dtos;

public static class QueryExtensions
{
    public static IQueryable<TDto> CastQueryable<TSource, TDto>(this IQueryable<TSource> queryable)
    {
        if (typeof(TSource) == typeof(TDto))
            return (IQueryable<TDto>)queryable;
        var visitor = new QueryExpressionVisitor<TSource, TDto>(queryable, null);
        return visitor.Translate();
    }

    public static async Task<PageList<TDto>> ToPageListAsync<TSource, TDto>(this IQueryable<TSource> queryable,
        IPageRequest<TDto> queryDto)
    {
        var query = TranslateQuery(queryable, queryDto);
        var total = await query.CountAsync();
        var items = new List<TDto>();
        if (total > 0)
        {
            items = await query.Take(queryDto.PageSize)
                .Skip(queryDto.PageSize * (queryDto.PageIndex - 1)).ToListAsync();
        }

        return new PageList<TDto>() { Total = total, Items = items };
    }

    public static async Task<TDto?> FirstOrDefaultAsync<TSource, TDto>(IQueryable<TSource> queryable,
        IPageRequest<TDto> queryDto)
    {
        var query = TranslateQuery(queryable, queryDto);
        return await query.FirstOrDefaultAsync();
    }

    private static IQueryable<TDto> TranslateQuery<TSource, TDto>(this IQueryable<TSource> queryable,
        IPageRequest<TDto> queryDto)
    {
        var visitor = new QueryExpressionVisitor<TSource, TDto>(queryable, queryDto);
        return visitor.Translate();
    }

}