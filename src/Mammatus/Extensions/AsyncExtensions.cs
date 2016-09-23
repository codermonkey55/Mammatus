using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mammatus.Extensions
{
    public static class AsyncExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task WhenAll(this IEnumerable<Task> tasks)
        {
            return Task.WhenAll(tasks);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks)
        {
            return Task.WhenAll(tasks);
        }

        /// <summary>
        /// 、
        /// </summary>
        /// <param name="task"></param>
        public static void FireAndForget(this Task task)
        {
            task.ContinueWith(x =>
            {
                if (x.Exception?.InnerException != null)
                    throw x.Exception?.InnerException;

            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="task"></param>
        /// <param name="onFaulted"></param>
        public static void FireAndForget(this Task task, Action<AggregateException> onFaulted)
        {
            task.ContinueWith(x =>
            {
                onFaulted?.Invoke(x.Exception);

                if (x.Exception?.InnerException != null)
                    throw x.Exception?.InnerException;

            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        /// <summary>
        /// http://neue.cc/2014/03/14_448.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <param name="concurrency"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="configureAwait"></param>
        /// <returns></returns>
        public static async Task ForEachAsync<T>
            (this IEnumerable<T> source, Func<T, Task> action, int concurrency,
            CancellationToken cancellationToken = default(CancellationToken), bool configureAwait = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (concurrency <= 0) throw new ArgumentOutOfRangeException("concurrency must be positive1");

            using (var semaphore = new SemaphoreSlim(initialCount: concurrency, maxCount: concurrency))
            {
                var exceptionCount = 0;
                var tasks = new List<Task>();

                foreach (var item in source)
                {
                    if (exceptionCount > 0) break;
                    cancellationToken.ThrowIfCancellationRequested();

                    await semaphore.WaitAsync(cancellationToken).ConfigureAwait(configureAwait);
                    var task = action(item).ContinueWith(t =>
                    {
                        semaphore?.Release();

                        if (t.IsFaulted)
                        {
                            Interlocked.Increment(ref exceptionCount);
                            if (t.Exception != null) throw t.Exception;
                        }
                    }, cancellationToken);
                    tasks.Add(task);
                }

                await Task.WhenAll(tasks.ToArray()).ConfigureAwait(configureAwait);
            }
        }

        public static async Task<IEnumerable<TResult>> SelectAsync<T, TResult>
            (this IEnumerable<T> source, Func<T, Task<TResult>> func, int concurrency,
            CancellationToken cancellationToken = default(CancellationToken), bool configureAwait = false)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));
            if (concurrency <= 0) throw new ArgumentOutOfRangeException("concurrency must be positive");

            using (var semaphore = new SemaphoreSlim(initialCount: concurrency, maxCount: concurrency))
            {
                var exceptionCount = 0;
                var tasks = new List<Task<TResult>>();

                foreach (var item in source)
                {
                    if (exceptionCount > 0) break;
                    cancellationToken.ThrowIfCancellationRequested();

                    await semaphore.WaitAsync(cancellationToken).ConfigureAwait(configureAwait);
                    var task = func(item).ContinueWith(t =>
                    {
                        semaphore?.Release();

                        if (t.IsFaulted)
                        {
                            Interlocked.Increment(ref exceptionCount);
                            throw t.Exception;
                        }
                        return t.Result;
                    }, cancellationToken);
                    tasks.Add(task);
                }

                return await Task.WhenAll(tasks.ToArray()).ConfigureAwait(configureAwait);
            }
        }
    }
}
