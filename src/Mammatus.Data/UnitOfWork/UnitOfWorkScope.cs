using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Mammatus.Data;
using Mammatus;
using Mammatus.Validation;
using Mammatus.ComponentModel;
using Mammatus.Core.IoC;
using Mammatus.ComponentModel.Containers;
using Mammatus.Data.Enums;
using Mammatus.Data.Helpers;
using Mammatus.Data.Infrastructure;

namespace Mammatus.Data.UnitOfWork
{
    using System;

    public class UnitOfWorkScope : IUnitOfWorkScope
    {
        private bool _disposed;

        private readonly Guid _scopeId = Guid.NewGuid();
        //private readonly ILog _logger = LogManager.GetLogger<UnitOfWorkScope>();
        private readonly IUnitOfWorkManager _uowManager;

        internal UnitOfWorkScope()
        {
            IInternalContainer containerAdapter = InternalContainer.Current;

            this.UnitOfWork = containerAdapter.GetInstance<IUnitOfWorkManager>().CurrentUnitOfWork;

            _uowManager = containerAdapter.GetInstance<IUnitOfWorkManager>();
        }

        internal UnitOfWorkScope(IUnitOfWork uow, IUnitOfWorkManager uowManager)
        {
            Guard.Against<ArgumentNullException>(uow == null, "The unit of work cannot be null.");
            Guard.Against<ArgumentNullException>(uowManager == null, "The unit of work manager cannot be null.");

            _uowManager = uowManager;
            this.UnitOfWork = uow;
        }

        #region Transaction
        public ITransaction BeginTransaction()
        {
            return BeginTransaction(TransactionMode.Default);
        }

        public ITransaction BeginTransaction(TransactionMode mode)
        {
            return BeginTransaction(UnitOfWorkSettings.DefaultIsolation, mode);
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return BeginTransaction(isolationLevel, TransactionMode.Default);
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel, TransactionMode mode)
        {
            Guard.Against<InvalidOperationException>(Transaction != null, "There is a active transcation, cannot begin a new transaction.");

            var scope = TransactionScopeHelper.CreateScope(isolationLevel, mode);

            var transaction = new UnitOfWorkTransaction(this, scope);

            return transaction;
        }

        #endregion

        /// <summary>
        /// Gets the unique Id of the <see cref="UnitOfWorkScope"/>.
        /// </summary>
        /// <value>A <see cref="Guid"/> representing the unique Id of the scope.</value>
        public Guid ScopeId
        {
            get { return _scopeId; }
        }

        /// <summary>
        /// Gets the current <see cref="IUnitOfWork"/> that the scope participates in.
        /// </summary>
        public IUnitOfWork UnitOfWork { get; private set; }

        internal ITransaction Transaction { get; private set; }

        ///<summary>
        /// Commits the current running transaction in the scope.
        ///</summary>
        public void Commit()
        {
            Guard.Against<ObjectDisposedException>(_disposed, "Cannot commit a disposed UnitOfWorkScope instance.");

            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
                Transaction = null;
            }
            else
            {
                UnitOfWork.Flush();
            }
        }

        #region IDisposable
        /// <summary>
        /// Disposes off the <see cref="UnitOfWorkScope"/> insance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes off the managed and un-managed resources used.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (Transaction != null)
                {
                    Transaction.Dispose();
                    Transaction = null;
                }

                if (UnitOfWork != null)
                {
                    UnitOfWork.Dispose();
                    UnitOfWork = null;
                }

                _disposed = true;
                _uowManager.EndScope(this);
            }
        }
        #endregion

        public static IUnitOfWorkScope BeginScope()
        {
            IInternalContainer containerAdapter = InternalContainer.Current;

            var uowManager = containerAdapter.GetInstance<IUnitOfWorkManager>();

            var scope = uowManager.BeginScope();

            return scope;
        }

        public static IUnitOfWorkScope CurrentScope
        {
            get
            {
                IInternalContainer containerAdapter = InternalContainer.Current;

                var uowManager = containerAdapter.GetInstance<IUnitOfWorkManager>();

                var scope = uowManager.CurrentScope;

                return scope;
            }
        }

        public static IUnitOfWork CurrentUnitOfWork
        {
            get { return CurrentScope.UnitOfWork; }
        }
    }
}
