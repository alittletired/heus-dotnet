using Heus.Ddd.Domain;
using Heus.Ddd.Query;


namespace Heus.Ddd.Dtos;

public static class QueryExtensions
{
    public static IQueryable< TDto> CastQueryable<TSource,TDto>(this IQueryable<TSource> queryable)
    {
        if (typeof(TSource) == typeof(TDto))
            return (IQueryable<TDto>)queryable;
        var visitor = new QueryExpressionVisitor<TSource, TDto>(queryable,null);
        return visitor.Translate();
    }

    public async static Task<PageList<TDto>> ToPageListAsync<TSource,TDto>(this IQueryable<TSource> queryable,
        IPageRequest<TDto> queryDto)
    {

        QueryExpressionVisitor<TSource,TDto> visitor = new(queryable, queryDto);
        var query = visitor.Translate();
        var total = await query.CountAsync();
        var items = new List<TDto>();
        if (total > 0)
        {
            items = await query.Take(queryDto.PageSize)
                .Skip(queryDto.PageSize * (queryDto.PageIndex - 1)).ToListAsync();
        }

        return new PageList<TDto>() { Total = total, Items = items };
    }

    public async static Task<TDto?> FirstOrDefaultAsync<TSource, TDto>(IQueryable<TSource> queryable, IPageRequest<TDto> queryDto) 
    {
        var query = TranslateQuery(queryable, queryDto);
        return await query.FirstOrDefaultAsync();
    }

    public async static Task<TDto> FirstAsync<TSource, TDto>(IQueryable<TSource> queryable, IPageRequest<TDto> queryDto) 
    {
        var query = TranslateQuery(queryable, queryDto);
        var data= await query.FirstOrDefaultAsync();
        if (data == null)
        {
            throw new EntityNotFoundException(typeof(TDto));
        }

        return data;
    }

    private static  IQueryable<TDto> TranslateQuery<TSource,TDto>(this IQueryable<TSource> queryable, IPageRequest<TDto> queryDto) 
    {
        var visitor = new QueryExpressionVisitor<TSource, TDto>(queryable, queryDto);
        return visitor.Translate();
    }

}