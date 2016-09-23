using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Mammatus.Enumerable.Extensions;

namespace Mammatus.Library.Collection
{
    public class ObservableSet<T> : INotifyCollectionChanged, INotifyPropertyChanged, IReadOnlyList<T>
    {

        private List<T> ItemsList { get; }
        private HashSet<T> ItemsSet { get; }

        public ObservableSet()
        {
            this.ItemsList = new List<T>();
            this.ItemsSet = new HashSet<T>();
        }
        public ObservableSet(IEnumerable<T> collection)
        {
            var enumerable = collection as T[] ?? collection.ToArray();
            this.ItemsList = new List<T>(enumerable);
            this.ItemsSet = new HashSet<T>(enumerable);
        }


        public bool Add(T item)
        {
            if (this.ItemsSet.Add(item))
            {
                this.ItemsList.Add(item);

                this.NotifyCountChanged();

                this.CollectionChanged?.Invoke(this,
                    new NotifyCollectionChangedEventArgs
                    (NotifyCollectionChangedAction.Add, item, this.ItemsList.Count - 1));

                return true;
            }
            return false;
        }

        public void AddRange(IEnumerable<T> items)
        {
            var added = items
                .Where(x => this.ItemsSet.Add(x))
                .ToList();

            if (added.Count <= 0)
            {
                return;
            }

            var index = this.ItemsList.Count;

            this.ItemsList.AddRange(added);

            this.NotifyCountChanged();

            this.CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs
                (NotifyCollectionChangedAction.Add, added, index));
        }

        public bool Remove(T item)
        {
            if (this.ItemsSet.Remove(item))
            {
                var index = this.ItemsList.FindIndex(x => x.Equals(item));
                if (index >= 0)
                {
                    this.ItemsList.RemoveAt(index);

                    this.NotifyCountChanged();

                    this.CollectionChanged?.Invoke(this,
                        new NotifyCollectionChangedEventArgs
                        (NotifyCollectionChangedAction.Remove, item, index));

                    return true;
                }
            }
            return false;
        }

        public void RemoveRange(int index, int count)
        {
            var removed = this.ItemsList.Skip(index).Take(count).ToArray();

            if (removed.Length <= 0)
            {
                return;
            }

            this.ItemsList.RemoveRange(index, count);
            removed.ForEach(x => this.ItemsSet.Remove(x));

            this.NotifyCountChanged();

            this.CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs
                (NotifyCollectionChangedAction.Remove, removed, index));
        }


        public void Clear()
        {
            this.ItemsSet.Clear();
            this.ItemsList.Clear();

            this.NotifyCountChanged();
            this.CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs
                (NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item)
        {
            return this.ItemsSet.Contains(item);
        }

        public bool Toggle(T item)
        {
            if (this.Add(item))
            {
                return true;
            }
            this.Remove(item);
            return false;
        }

        private void NotifyCountChanged()
        {
            this.RaisePropertyChanged(nameof(Count));
        }

        public int Count => this.ItemsList.Count;
        public T this[int index] => this.ItemsList[index];

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public IEnumerator<T> GetEnumerator()
            => this.ItemsList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable<T>)this).GetEnumerator();
    }
}
