using System;

namespace Mammatus.Core.Application
{
    public class Disposable : IDisposable
    {
        #region Implementation of the IDisposable Pattern

        private bool _alreadyDisposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>
        /// tdill - 4/9/2008 12:50 PM
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <remarks>
        /// tdill - 4/9/2008 12:50 PM
        /// </remarks>
        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDisposed) return;
            if (isDisposing)
            {
                //TODO: free managed resources here.
            }
            // TODO:  free unmanaged resources here.
            _alreadyDisposed = true;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Disposal"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// tdill - 4/9/2008 12:51 PM
        /// </remarks>
        ~Disposable()
        {
            Dispose(false);
        }

        #endregion
    }
}
