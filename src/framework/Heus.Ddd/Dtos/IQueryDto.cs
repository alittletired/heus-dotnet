

namespace Heus.Ddd.Dtos
{
    public interface IQueryDto<T>
    {
         int PageIndex { get;  }
         int PageSize { get;  } 
    }
}
