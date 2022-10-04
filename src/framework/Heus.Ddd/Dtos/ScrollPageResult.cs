namespace Heus.Ddd.Dtos;

public class ScrollPageResult<T>
{
    public string? ScrollId { get; }
    public IEnumerable<T> Items { get; }

    public ScrollPageResult(IEnumerable<T> items, string? scrollId)
    {
        ScrollId = scrollId;
        Items = items;
    }
    
}