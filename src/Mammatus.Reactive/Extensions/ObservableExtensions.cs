using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Mammatus.Reactive.Extensions
{
    public static class ObservableExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="disposable"></param>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T AddTo<T, TKey>
            (this T disposable, IDictionary<TKey, IDisposable> dictionary, TKey key) where T : IDisposable
        {
            IDisposable result;
            if (dictionary.TryGetValue(key, out result))
            {
                result?.Dispose();
                dictionary.Remove(key);
            }

            dictionary.Add(key, disposable);

            return disposable;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static IObservable<T> DownSample<T>(this IObservable<T> source, TimeSpan interval)
        {
            return Observable.Create<T>(o =>
            {
                var subscriptions = new CompositeDisposable();
                var acceepted = true;

                var pub = source.Where(x => acceepted);

                pub.Subscribe(o).AddTo(subscriptions);

                pub.Do(x => acceepted = false)
                .Delay(interval).Subscribe(x => acceepted = true)
                .AddTo(subscriptions);

                return subscriptions;
            });
        }

        public static IObservable<T> Restrict<T>(this IObservable<T> source, TimeSpan interval)
        {
            return source.Restrict(interval, Scheduler.Default);
        }

        public static IObservable<T> Restrict<T>(this IObservable<T> source, TimeSpan interval, IScheduler scheduler)
        {
            object _gate = new object();
            T _value = default(T);
            bool _hasValue = false;
            SerialDisposable _cancelable = new SerialDisposable();
            ulong lastId = 0UL;

            return Observable.Create<T>(observer =>
            {
                return source.Subscribe(value =>
                {

                    var currentid = default(ulong);

                    lock (_gate)
                    {
                        var isIdle = !_hasValue;

                        _hasValue = true;
                        _value = value;

                        if (isIdle)
                        {
                            currentid = lastId;
                            lastId = unchecked(lastId + 1);
                            observer.OnNext(_value);
                        }
                        else
                        {
                            lastId = unchecked(lastId + 1);
                            currentid = lastId;
                        }
                    }

                    var d = new SingleAssignmentDisposable();
                    _cancelable.Disposable = d;

                    d.Disposable = scheduler.Schedule(currentid, interval, (IScheduler self, ulong id) =>
                    {
                        lock (_gate)
                        {
                            if (_hasValue && lastId == id)
                            {
                                observer.OnNext(_value);
                            }
                            _hasValue = false;
                        }

                        return Disposable.Empty;
                    });

                });
            });

        }

        public static ReactiveCommand<Tvalue> WithSubscribe<Tvalue>
            (this ReactiveCommand<Tvalue> observable, Action<Tvalue> action, ICollection<IDisposable> container)
        {
            observable.Subscribe(action).AddTo(container);
            observable.AddTo(container);
            return observable;
        }

        public static ReactiveCommand WithSubscribe
            (this ReactiveCommand observable, Action<object> action, ICollection<IDisposable> container)
        {
            observable.Subscribe(action).AddTo(container);
            observable.AddTo(container);
            return observable;
        }

        public static ReactiveCommand WithSubscribeOfType<T>
            (this ReactiveCommand observable, Action<T> action, ICollection<IDisposable> container)
        {
            observable.OfType<T>().Subscribe(action).AddTo(container);
            observable.AddTo(container);
            return observable;
        }

        public static Subject<Tvalue> WithSubscribe<Tvalue>
            (this Subject<Tvalue> observable, Action<Tvalue> action, ICollection<IDisposable> container)
        {
            observable.Subscribe(action).AddTo(container);
            observable.AddTo(container);
            return observable;
        }

        public static bool Toggle(this ReactiveProperty<bool> target)
        {
            var newValue = !target.Value;
            target.Value = newValue;
            return newValue;
        }

        public static IObservable<T> SkipAfter<T, V>(this IObservable<T> source, IObservable<V> trigger, int count)
        {
            return Observable.Create<T>(o =>
            {
                var subscriptions = new CompositeDisposable();

                var acceptCount = 0;

                trigger.Subscribe(_ => acceptCount = count).AddTo(subscriptions);

                source.Where(_ => --acceptCount < 0).Subscribe(o).AddTo(subscriptions);

                return subscriptions;
            });
        }

        public static IObservable<IList<T>> BufferUntilThrottle<T>
            (this IObservable<T> source, double timeMilliseconds)
        {
            return source.BufferUntilThrottle(timeMilliseconds, true);
        }

        public static IObservable<IList<T>> BufferUntilThrottle<T>
            (this IObservable<T> source, double timeMilliseconds, bool publish)
        {
            var observable = source;

            if (publish)
            {
                observable = source.Publish().RefCount();
            }

            return observable
                .Buffer(observable.Throttle(TimeSpan.FromMilliseconds(timeMilliseconds)))
                .Where(x => x.Count > 0);
        }

        /// <summary>
        /// Projects old and new element of a sequence into a new form.
        /// </summary>
        public static IObservable<OldNewPair<T>> Pairwise<T>(this IObservable<T> source, T initialValue) =>
            Pairwise(source, initialValue, (x, y) => new OldNewPair<T>(x, y));

        /// <summary>
        /// Projects old and new element of a sequence into a new form.
        /// </summary>
        public static IObservable<TR> Pairwise<T, TR>(this IObservable<T> source, T initialValue, Func<T, T, TR> selector)
        {
            var result = Observable.Create<TR>(observer =>
            {
                T prev = initialValue;

                return source.Subscribe(x =>
                {

                    TR value;
                    try
                    {
                        value = selector(prev, x);
                        prev = x;
                    }
                    catch (Exception ex)
                    {
                        observer.OnError(ex);
                        return;
                    }

                    observer.OnNext(value);
                }, observer.OnError, observer.OnCompleted);
            });

            return result;
        }
    }
}
