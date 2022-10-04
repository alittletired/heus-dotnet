
namespace Heus.Ddd.Dtos;
public class PagedList<T>
{
    public int Total { get; }
    public IEnumerable<T> Items { get; }

    /// <summary>
    /// Creates a new <see cref="PagedList{T}"/> object.
    /// </summary>
    /// <param name="total">Total count of Items</param>
    /// <param name="items">List of items in current page</param>
    public PagedList(int total, IEnumerable<T> items)
    {
        Total = total;
        Items = items;
    }
}
