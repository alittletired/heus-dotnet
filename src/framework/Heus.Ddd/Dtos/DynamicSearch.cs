
using System.Text.Json;

namespace Heus.Ddd.Dtos;

public record DynamicSearchFilter(string Op, object Value, string? Field)
{
  
  
}
public class DynamicSearch<T>: IPageRequest<T>
{
    public DynamicSearch()
    {
       
    }


    public Dictionary<string, DynamicSearchFilter> Filters { get; set; } = new();
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }
   
   
}