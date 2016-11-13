﻿using Mammatus.Domain.UnitOfWork;
using Mammatus.Exceptions;
using NHibernate;
using System;
using System.Transactions;

namespace Mammatus.Data.NHibernate.DbOperations
{
    public class NHUnitOfWork : INHUnitOfWork
    {
        private ISession session;
        private readonly NHUnitOfWorkFactory _factory;

        public NHUnitOfWork(ISession session, IUnitOfWork previous, NHUnitOfWorkFactory factory)
        {
            this._factory = factory;
            this.session = session;
            this.Previous = previous;
        }

        public IUnitOfWork Previous { get; private set; }

        public ISession Session { get { return this.session; } }

        /// <summary>
        /// Commit all changes made in  a container.
        /// </summary>
        public void Commit()
        {
            try
            {
                this.session.Transaction.Commit();
            }
            catch (StaleObjectStateException ex)
            {
                throw new ConcurrencyException("Object was edited or deleted by another transaction", ex);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Rollback changes not stored in databse at
        /// this moment. See references of UnitOfWork pattern
        /// </summary>
        public void RollbackChanges()
        {
            this.session.Transaction.Rollback();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.session != null && this.session.IsOpen)
                {
                    if (this.session.Transaction != null)
                    {
                        if (this.session.Transaction.IsActive)
                        {
                            if (Transaction.Current == null)
                            {
                                this.session.Transaction.Rollback();
                            }
                        }

                        this.session.Transaction.Dispose();
                    }

                    this.session.Dispose();
                    this.session = null;
                    this._factory.UpdateCurrent(this);
                }
            }
        }
    }
}