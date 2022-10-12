
namespace Heus.Ddd.Dtos;

public class DynamicQuery<T>: IQueryDto<T>
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? OrderBy { get; set; }
    public Dictionary<string,object?> Filters { get; set; } = new(StringComparer.OrdinalIgnoreCase);

}