using System;

namespace Mammatus.Library.Async
{
    public sealed class Releaser : IDisposable
    {
        private AsyncLock _target;
        private readonly Action _setIsLockedFalse;
        private readonly Action _setReleaseSemaphore;

        internal Releaser(AsyncLock obj, Action setReleaseSemaphore, Action setIsLockedFalse)
        {
            this._target = obj;
            this._setReleaseSemaphore = setReleaseSemaphore;
            this._setIsLockedFalse = setIsLockedFalse;
        }

        public void Dispose()
        {
            _target = null;
            _setIsLockedFalse();
            _setReleaseSemaphore();
        }
    }

}