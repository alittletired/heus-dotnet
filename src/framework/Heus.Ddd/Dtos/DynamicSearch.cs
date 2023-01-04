
using System.Linq.Expressions;
using System.Text.Json;
using System.Xml.Linq;
using Heus.Core.Utils;
using Heus.Ddd.Query;

namespace Heus.Ddd.Dtos;

public record DynamicSearchFilter(string Op, object Value, string? Field)
{
  
  
}
public class DynamicSearch<T> : IPageRequest<T>
{
    public DynamicSearch()
    {

    }
    public DynamicSearch<T> AddEqualFilter<TResult>(Expression<Func<T, TResult>> selector, TResult value, string? filed = null) where TResult : notnull
    {
        return AddFilter(selector, OperatorTypes.Equal, value, filed);
    }
    public DynamicSearch<T> AddGreaterThanFilter<TResult>(Expression<Func<T, TResult>> selector, TResult value, string? filed = null) where TResult : notnull
    {
        return AddFilter(selector, OperatorTypes.GreaterThan, value, filed);
    }
    public DynamicSearch<T> AddGreaterOrEqualFilter<TResult>(Expression<Func<T, TResult>> selector, TResult value, string? filed = null) where TResult : notnull
    {
        return AddFilter(selector, OperatorTypes.GreaterOrEqual, value, filed);
    }
    public DynamicSearch<T> AddLessThanFilter<TResult>(Expression<Func<T, TResult>> selector, TResult value, string? filed = null) where TResult : notnull
    {
        return AddFilter(selector, OperatorTypes.LessThan, value, filed);
    }
    public DynamicSearch<T> AddLessOrEqualFilter<TResult>(Expression<Func<T, TResult>> selector, TResult value, string? filed = null) where TResult : notnull
    {
        return AddFilter(selector, OperatorTypes.LessOrEqual, value, filed);
    }

    public DynamicSearch<T> AddHeadLikeFilter(Expression<Func<T, string>> selector, string value, string? filed = null)
    {
        return AddFilter(selector, OperatorTypes.HeadLike, value, filed);
    }
    public DynamicSearch<T> AddTailLikeFilter(Expression<Func<T, string>> selector, string value, string? filed = null)
    {
        return AddFilter(selector, OperatorTypes.TailLike, value, filed);
    }

    public DynamicSearch<T> AddLikeFilter(Expression<Func<T, string>> selector, string value, string? filed = null)
    {
        return AddFilter(selector, OperatorTypes.Like, value, filed);
    }

    public DynamicSearch<T> AddInFilter<TResult>(Expression<Func<T, TResult>> selector, IEnumerable<TResult> value, string? filed = null) where TResult : notnull
    {
        var property = ExpressionUtils.GetPropertyInfo(selector);
        Filters[property.Name] = new DynamicSearchFilter(OperatorTypes.In, value, filed); ;
        return this;
    }

    public DynamicSearch<T> AddNotInFilter<TResult>(Expression<Func<T, TResult>> selector, IEnumerable<TResult> value, string? filed = null) where TResult : notnull
    {
        var property = ExpressionUtils.GetPropertyInfo(selector);
        Filters[property.Name] = new DynamicSearchFilter(OperatorTypes.NotIn, value, filed); ;
        return this;
    }

    public DynamicSearch<T> AddFilter<TResult>(Expression<Func<T, TResult>> selector, string operatorTypes, TResult value, string? filed = null) where TResult : notnull
    {
        var property = ExpressionUtils.GetPropertyInfo(selector);
        return AddFilter(property.Name, operatorTypes, value, filed);
     
    }
    public DynamicSearch<T> AddFilter<TResult>(string propertyName, string operatorTypes, TResult value, string? filed = null) where TResult : notnull
    {
        Filters[propertyName] = new DynamicSearchFilter(operatorTypes, value, filed); ;
        return this;
    }
    public Dictionary<string, DynamicSearchFilter> Filters { get; set; } = new();
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }


}