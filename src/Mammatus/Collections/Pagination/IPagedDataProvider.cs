using System.Collections.Generic;

namespace Mammatus.Collections.Pagination
{
    public interface IPagedDataProvider<T>
    {
        IPagedData<T> GetPagedData(IEnumerable<T> items, int currentPage, int itemsPerPage);

        IPagedData<T> GetPagedData(IEnumerable<T> items, int totalCountOfItems, int currentPage, int itemsPerPage);
    }
}
