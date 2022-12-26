namespace Heus.Ddd.Dtos;

    public class ScrollPageRequest
    {
       public long? ScrollId { get; set; }
       public int PageSize { get; set; } = 10;
    }

