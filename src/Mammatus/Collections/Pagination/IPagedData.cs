using System.Collections.Generic;

namespace Mammatus.Collections.Pagination
{
    public interface IPagedData<T> : IEnumerable<T>
    {
        int TotalCountOfItems { get; set; }
        int CurrentPage { get; set; }
        int ItemsPerPage { get; set; }
        int TotalPages { get; set; }

        bool HasNextPage { get; set; }
        bool HasPreviousPage { get; set; }

        int NextPage { get; }
        int PreviousPage { get; }

        List<T> Items { get; }
    }
}
