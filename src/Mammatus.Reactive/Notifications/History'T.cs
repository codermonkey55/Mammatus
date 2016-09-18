using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Mammatus.Reactive.Notifications
{
    public class History<T> : DisposableBase
    {
        private ReactiveProperty<T> CurrentState { get; }

        public IObservable<T> StateChanged => this.CurrentState.AsObservable();

        public T Current => this.CurrentState.Value;

        private ObservableCollection<T> BackHistory { get; }

        private ObservableCollection<T> ForwardHistory { get; }

        public ReadOnlyReactiveProperty<int> BackHistoryCount { get; }

        public ReadOnlyReactiveProperty<int> ForwardHistoryCount { get; }

        public Func<T, T, bool> EqualityChecker { get; set; }


        public History(T initialValue)
        {

            this.BackHistory = new ObservableCollection<T>();
            this.ForwardHistory = new ObservableCollection<T>();

            this.CurrentState = new ReactiveProperty<T>(initialValue).AddTo(this.Disposables);

            this.BackHistoryCount = this.BackHistory
                .CollectionChangedAsObservable()
                .Select(x => this.BackHistory.Count)
                .DistinctUntilChanged()
                .ToReadOnlyReactiveProperty(0)
                .AddTo(this.Disposables);

            this.ForwardHistoryCount = this.ForwardHistory
                .CollectionChangedAsObservable()
                .Select(x => this.ForwardHistory.Count)
                .DistinctUntilChanged()
                .ToReadOnlyReactiveProperty(0)
                .AddTo(this.Disposables);
        }

        public void MoveBack()
        {
            var length = this.BackHistory.Count;
            if (length <= 0)
            {
                return;
            }
            var item = this.BackHistory[length - 1];

            this.ForwardHistory.Add(this.CurrentState.Value);
            this.BackHistory.RemoveAt(length - 1);
            this.CurrentState.Value = item;
        }

        public void MoveForward()
        {
            var length = this.ForwardHistory.Count;
            if (length <= 0)
            {
                return;
            }
            var item = this.ForwardHistory[length - 1];

            this.BackHistory.Add(this.CurrentState.Value);
            this.ForwardHistory.RemoveAt(length - 1);
            this.CurrentState.Value = item;
        }

        public void MoveNew(T item)
        {
            if (this.EqualityChecker == null)
            {
                this.EqualityChecker = (a, b) => a.Equals(b);
            }

            var length = this.ForwardHistory.Count;
            if (length > 0 && this.EqualityChecker(this.ForwardHistory[length - 1], item))
            {
                this.MoveForward();
            }
            else
            {
                if (this.CurrentState.Value != null)
                {
                    this.BackHistory.Add(this.CurrentState.Value);
                }
                this.ForwardHistory.Clear();
                this.CurrentState.Value = item;
            }
        }

        public void InsertToBack(T item)
        {
            this.BackHistory.Add(item);
        }

        public void Clear()
        {
            this.CurrentState.Value = default(T);
            this.BackHistory.Clear();
        }
    }
}
