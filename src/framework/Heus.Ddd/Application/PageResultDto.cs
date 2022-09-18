
namespace Heus.Ddd.Application;

    public class PagedResultDto<T>
    {
        public int TotalCount { get;  } 
        public IEnumerable<T> Items { get;  }
        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PagedResultDto(int totalCount, IEnumerable<T> items)

        {
            TotalCount = totalCount;
            Items = items;
        }
    }
