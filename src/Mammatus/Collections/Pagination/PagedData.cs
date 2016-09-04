using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mammatus.Collections.Pagination
{
    public class PagedData<T> : IPagedData<T>, IPagedDataProvider<T>
    {
        protected IEnumerable<T> CurrentItems;

        protected ArrayList InnerArray;

        public static int DefaultPageSize => 20;

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
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        public int PreviousPage
        {
            get
            {
                if (!HasPreviousPage)
                    return CurrentPage;

                return CurrentPage - 1;
            }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        public List<T> Items
        {
            get { return InnerArray.Cast<T>().ToList(); }
            set { CurrentItems = value; }
        }


        public PagedData()
        {
            InnerArray = new ArrayList();
        }

        public PagedData(IEnumerable<T> currentItems, int currentPage, int itemsPerPage)
        {
            if (InnerArray == null) InnerArray = new ArrayList();

            var enumerable = currentItems as T[] ?? currentItems.ToArray();

            InnerArray.AddRange(enumerable);

            TotalCountOfItems = enumerable.Length;

            CurrentPage = currentPage;

            ItemsPerPage = itemsPerPage;

            TotalPages = (int)Math.Ceiling((float)TotalCountOfItems / ItemsPerPage);

            HasNextPage = CurrentPage < TotalPages;

            HasPreviousPage = CurrentPage > 1;
        }


        public PagedData(IEnumerable<T> currentItems, int totalCountOfItems, int currentPage, int itemsPerPage)
        {
            if (InnerArray == null) InnerArray = new ArrayList();

            InnerArray.AddRange(currentItems.ToArray());

            TotalCountOfItems = totalCountOfItems;

            CurrentPage = currentPage;

            ItemsPerPage = itemsPerPage;

            TotalPages = (int)Math.Ceiling((float)TotalCountOfItems / ItemsPerPage);

            HasNextPage = CurrentPage < TotalPages;

            HasPreviousPage = CurrentPage > 1;
        }

        public IPagedData<T> GetPagedData(IEnumerable<T> items, int currentPage, int itemsPerPage)
        {
            var enumerable = items as T[] ?? items.ToArray();

            IEnumerable<T> pagedItems = enumerable.Skip((currentPage - 1) * itemsPerPage)
                                                  .Take(itemsPerPage)
                                                  .Select(x => x).ToArray();

            return new PagedData<T>(pagedItems, enumerable.Length, currentPage, itemsPerPage);
        }

        public IPagedData<T> GetPagedData(IEnumerable<T> items, int totalCountOfItems, int currentPage, int itemsPerPage)
        {
            IEnumerable<T> pagedItems = items.Skip((currentPage - 1) * itemsPerPage)
                                             .Take(itemsPerPage)
                                             .Select(x => x).ToArray();

            return new PagedData<T>(pagedItems, totalCountOfItems, currentPage, itemsPerPage);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var enumator = InnerArray.Cast<T>().GetEnumerator();

            return enumator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var enumator = InnerArray.GetEnumerator();

            return enumator;
        }

        public virtual T this[int index]
        {
            get
            {
                return (T)InnerArray[index];
            }
            set
            {
                InnerArray[index] = value;
            }
        }

        public virtual void Add(T item)
        {
            InnerArray.Add(item);
        }

        public virtual bool Remove(T item)
        {
            bool result = false;

            for (int i = 0; i < InnerArray.Count; i++)
            {
                T obj = (T)InnerArray[i];

                if (obj.Equals(item))
                {
                    InnerArray.RemoveAt(i);

                    result = true;

                    break;
                }
            }

            return result;
        }

        public void Clear()
        {
            InnerArray.Clear();
        }

        public bool Contains(T item)
        {
            foreach (T obj in InnerArray)
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
            InnerArray.CopyTo(array, arrayIndex);
        }

        public int Count => InnerArray.Count;

        public bool IsReadOnly => InnerArray.IsReadOnly;

        public int IndexOf(T item)
        {
            return InnerArray.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            InnerArray.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            InnerArray.RemoveAt(index);
        }
    }
}