
namespace Heus.Ddd.Dtos;
public class PageList<T>
{
    public int Total { get; set; }
    public IEnumerable<T> Items { get; set; }=new List<T>();

    /// <summary>
    /// Creates a new <see cref="PageList{T}"/> object.
    /// </summary>
    /// <param name="total">Total count of Items</param>
    /// <param name="items">List of items in current page</param>
    public PageList(int total, IEnumerable<T> items)
    {
        Total = total;
        Items = items;
    }
    public PageList() { }
}
