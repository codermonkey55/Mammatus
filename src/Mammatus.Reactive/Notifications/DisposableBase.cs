using System;
using System.Reactive.Disposables;

namespace Mammatus.Reactive.Notifications
{
    public class DisposableBase : IDisposable
    {
        public bool IsDisposed => this._disposables.IsDisposed;

        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        protected CompositeDisposable Disposables => this._disposables;

        public virtual void Dispose() => this._disposables.Dispose();
    }
}
