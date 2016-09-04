using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mammatus.Collections.Pagination
{
    public class PagedData<T> : IPagedData<T>, IPagedDataProvider<T>
    {
        protected IEnumerable<T> _currentItems;

        protected ArrayList _innerArray;

        public static int DefaultPageSize { get { return 20; } }

        public int TotalCountOfItems { get; set; }

        public int CurrentPage { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalPages { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public int NextPage
        {
            get
            {
                if (!HasNextPage)
                    return CurrentPage;

                return CurrentPage + 1;
            }
            set { }
        }

        public int PreviousPage
        {
            get
            {
                if (!HasPreviousPage)
                    return CurrentPage;

                return CurrentPage - 1;
            }
            set { }
        }

        public List<T> Items
        {
            get { return _innerArray.Cast<T>().ToArray().ToList(); }
            set { }
        }


        public PagedData()
        {
            _innerArray = new ArrayList();
        }

        public PagedData(IEnumerable<T> currentItems, int currentPage, int itemsPerPage)
        {
            if (_innerArray == null) _innerArray = new ArrayList();

            _innerArray.AddRange(currentItems.ToArray());

            TotalCountOfItems = currentItems.Count();

            CurrentPage = currentPage;

            ItemsPerPage = itemsPerPage;

            TotalPages = (int)Math.Ceiling((float)TotalCountOfItems / ItemsPerPage);

            HasNextPage = CurrentPage < TotalPages;

            HasPreviousPage = CurrentPage > 1;
        }


        public PagedData(IEnumerable<T> currentItems, int totalCountOfItems,int currentPage, int itemsPerPage)
        {
            if (_innerArray == null) _innerArray = new ArrayList();

            _innerArray.AddRange(currentItems.ToArray());

            TotalCountOfItems = totalCountOfItems;

            CurrentPage = currentPage;

            ItemsPerPage = itemsPerPage;

            TotalPages = (int) Math.Ceiling((float) TotalCountOfItems/ItemsPerPage);

            HasNextPage = CurrentPage < TotalPages;

            HasPreviousPage = CurrentPage > 1;
        }

        public IPagedData<T> GetPagedData(IEnumerable<T> items, int currentPage, int itemsPerPage)
        {
            IEnumerable<T> pagedItems = null;

            pagedItems = items.Skip((currentPage - 1) * itemsPerPage)
                              .Take(itemsPerPage)
                              .Select(x => x).ToArray();

            return new PagedData<T>(pagedItems, items.Count(), currentPage, itemsPerPage);
        }

        public IPagedData<T> GetPagedData(IEnumerable<T> items, int totalCountOfItems, int currentPage, int itemsPerPage)
        {
            IEnumerable<T> pagedItems = null;

            pagedItems = items.Skip((currentPage - 1) * itemsPerPage)
                              .Take(itemsPerPage)
                              .Select(x => x).ToArray();

            return new PagedData<T>(pagedItems, totalCountOfItems, currentPage, itemsPerPage);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var enumator = _innerArray.Cast<T>().GetEnumerator();

            return enumator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var enumator = _innerArray.GetEnumerator();

            return enumator;
        }

        public virtual T this[int index]
        {
            get
            {
                return (T)_innerArray[index];
            }
            set
            {
                _innerArray[index] = value;
            }
        }

        public virtual void Add(T item)
        {
            _innerArray.Add(item);
        }

        public virtual bool Remove(T item)
        {
            bool result = false;

            for (int i = 0; i < _innerArray.Count; i++)
            {
                T obj = (T)_innerArray[i];

                if (obj.Equals(item))
                {
                    _innerArray.RemoveAt(i);

                    result = true;

                    break;
                }
            }

            return result;
        }

        public void Clear()
        {
            _innerArray.Clear();
        }

        public bool Contains(T item)
        {
            foreach (T obj in _innerArray)
            {
                if (obj.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _innerArray.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _innerArray.Count; }
        }

        public bool IsReadOnly
        {
            get { return _innerArray.IsReadOnly; }
        }

        public int IndexOf(T item)
        {
            return _innerArray.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _innerArray.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _innerArray.RemoveAt(index);
        }
    }
}