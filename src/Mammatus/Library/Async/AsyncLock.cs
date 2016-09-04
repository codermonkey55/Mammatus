using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mammatus.Library.Async
{
    public sealed class AsyncLock
    {
        private readonly SemaphoreSlim _semaphore;

        private readonly Task<IDisposable> _releaser;
        public bool IsLocked { get; private set; }


        public AsyncLock(int semaphoreCount)
        {
            this._semaphore = new SemaphoreSlim(semaphoreCount, semaphoreCount);
            this.IsLocked = false;
            this._releaser = Task.FromResult((IDisposable)new Releaser(this, SetIsLockedFalse, SetReleaseSemaphore));
        }

        public AsyncLock() : this(1)
        {
        }

        public Task<IDisposable> LockAsync()
        {
            var wait = this._semaphore.WaitAsync();

            if (wait.IsCompleted)
            {
                this.IsLocked = true;
                return this._releaser;
            }

            return wait.ContinueWith(
                (_, state) =>
                {
                    this.IsLocked = true;
                    return (IDisposable)state;
                },
                this._releaser.Result,
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default
                );
        }

        private void SetIsLockedFalse() => this.IsLocked = false;

        private void SetReleaseSemaphore() => this._semaphore.Release();
    }
}
