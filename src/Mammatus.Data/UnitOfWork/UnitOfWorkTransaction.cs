using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Mammatus;
using Mammatus.Code.Contracts;
using Mammatus.Data.Infrastructure;

namespace Mammatus.Data.UnitOfWork
{
    using System;

    public class UnitOfWorkTransaction : ITransaction
    {
        private bool _completed;
        private bool _commitAttempted;
        private bool _disposed;

        private readonly Guid _transactionId = Guid.NewGuid();
        private readonly TransactionScope _transactionScope;
        private readonly IUnitOfWorkScope _uowScope;
        //private readonly ILog _logger = LogManager.GetLogger<UnitOfWorkTransaction>();

        public UnitOfWorkTransaction(IUnitOfWorkScope uowScope, TransactionScope transactionScope)
        {
            CodeContract.Require(uowScope != null, "The unit of work scope was null");
            CodeContract.Require(transactionScope != null, "The transaction scope was null");

            _uowScope = uowScope;
            _transactionScope = transactionScope;
        }

        ///<summary>
        /// Gets the unique transaction id of the <see cref="UnitOfWorkTransaction"/> instance.
        ///</summary>
        /// <value>A <see cref="Guid"/> representing the unique id of the <see cref="UnitOfWorkTransaction"/> instance.</value>
        public Guid TransactionId
        {
            get { return _transactionId; }
        }

        public void Commit()
        {
            _commitAttempted = true;
            try
            {
                _uowScope.UnitOfWork.Flush();
                _transactionScope.Complete();
                _completed = true;
            }
            finally
            {
                Dispose();
                //Dispose the transaction after commit.
            }
        }

        public void Rollback()
        {
            //_logger.Info(x => x("UnitOfWorkTransaction {0} Rolling back.", _transactionId));
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
                try
                {
                    if (_completed)
                    {
                        //Scope is marked as completed. Nothing to do here...
                        _disposed = true;
                        return;
                    }

                    if (!_commitAttempted && UnitOfWorkSettings.AutoCompleteScope)
                        //Scope did not try to commit before, and auto complete is switched on. Trying to commit.
                        //If an exception occurs here, the finally block will clean things up for us.
                        Commit();
                    else
                        //Scope either tried a commit before or auto complete is turned off. Trying to rollback.
                        //If an exception occurs here, the finally block will clean things up for us.
                        Rollback();
                }
                finally
                {
                    _transactionScope.Dispose();
                    _disposed = true;
                }
            }
        }
        #endregion
    }
}
