using System;
using System.Transactions;
using Mammatus.Data.Enums;

namespace Mammatus.Data.Helpers
{
        public static class TransactionScopeHelper
    {
        //static readonly ILog Logger = LogManager.GetLogger(typeof (TransactionScopeHelper));

        public static TransactionScope CreateScope(IsolationLevel isolationLevel, TransactionMode txMode)
        {
            if (txMode == TransactionMode.New)
            {
                //Logger.Debug(x => x("Creating a new TransactionScope with TransactionScopeOption.RequiresNew"));

                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = isolationLevel });
            }
            if (txMode == TransactionMode.Supress)
            {
                //Logger.Debug(x => x("Creating a new TransactionScope with TransactionScopeOption.Supress"));

                return new TransactionScope(TransactionScopeOption.Suppress);
            }

            //Logger.Debug(x => x("Creating a new TransactionScope with TransactionScopeOption.Required"));

            return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = isolationLevel });
        }
    }
}