
namespace Heus.Ddd.Dtos;
public record  PageList<T>
{
    public required int Total { get; init; }
    public  required IEnumerable<T> Items { get; init; }
   
}
