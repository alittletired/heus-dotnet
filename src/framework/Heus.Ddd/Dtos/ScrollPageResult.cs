namespace Heus.Ddd.Dtos;

public class ScrollPageResult<T>
{
    public IEnumerable<T> Items { get; }
    public ScrollPageResult(IEnumerable<T> items)
    {
        Items = items;
    }
    
}