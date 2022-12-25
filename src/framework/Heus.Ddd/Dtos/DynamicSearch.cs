
using System.Linq.Expressions;
using System.Text.Json;
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
        var property = ExpressionUtils.GetPropertyInfo(selector);
        Filters[property.Name] = new DynamicSearchFilter(OperatorTypes.Equal, value, filed); ;
        return this;
    }

    public Dictionary<string, DynamicSearchFilter> Filters { get; set; } = new();
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }


}