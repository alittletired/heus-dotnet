

namespace Heus.Ddd.Dtos
{
    public interface IPageRequest<T>
    {
         int PageIndex { get;  }
         int PageSize { get;  } 
    }
}
