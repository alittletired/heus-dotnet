using System.Linq.Expressions;

namespace Heus.Ddd.Dtos;

public class DynamicQuery<T>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }

    public Expression<Func<TEntity, TEntity2, bool>> IsSatisfied<TEntity, TEntity2>(TEntity entity, TEntity2 entity2)
    {
        var p0 = Expression.Parameter(typeof(TEntity), "p0");
        var five = Expression.Constant(5, typeof(int));
        var numLessThanFive = Expression.LessThan(p0, five);
        var lambda = Expression.Lambda<Func<TEntity, TEntity2, bool>>(
                numLessThanFive, p0);
        return lambda;
    }
}