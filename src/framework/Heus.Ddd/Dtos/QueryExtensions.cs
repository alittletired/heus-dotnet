using Heus.Ddd.Domain;
using Heus.Ddd.Query;


namespace Heus.Ddd.Dtos;

public static class QueryExtensions
{
    public static IQueryable<TDto> CastQueryable<TDto>(this IQueryable queryable)
    {
       
        QueryExpressionVisitor<TDto> visitor = new(queryable);
        return visitor.Translate(null);
    }

    public async static Task<PageList<TDto>> ToPageListAsync<TDto>(this IQueryable queryable,
        IPageRequest<TDto> queryDto)
    {

        QueryExpressionVisitor<TDto> visitor = new(queryable);
        var query = visitor.Translate(queryDto);
        var total = await query.CountAsync();
        var items = new List<TDto>();
        if (total > 0)
        {
            items = await query.Take(queryDto.PageSize)
                .Skip(queryDto.PageSize * (queryDto.PageIndex - 1)).ToListAsync();
        }

        return new PageList<TDto>() { Total = total, Items = items };
    }

    // public static async Task<IEnumerable<T>> ToDtoList<T>(this IQueryable queryable)
    // {
    //     var query = TranslateQuery(queryable, queryDto);
    // }

    public async static Task<T?> FirstOrDefaultAsync<T>(IQueryable queryable, IPageRequest<T> queryDto) 
    {
        var query = TranslateQuery(queryable, queryDto);
        return await query.FirstOrDefaultAsync();
    }

    public async static Task<T> First<T>(IQueryable queryable, IPageRequest<T> queryDto) 
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