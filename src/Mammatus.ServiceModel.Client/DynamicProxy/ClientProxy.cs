using System;

namespace Mammatus.ServiceModel.Client.DynamicProxy
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ClientProxy : IClientProxy
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        protected internal ClientProxy()
        {

        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// 
        /// </summary>
        protected internal abstract void CloseConnectionForClientProxy();

        #endregion

        #region Fields

        /// <summary>
        /// 
        /// </summary>
        protected internal Boolean isInternalCall = false;

        /// <summary>
        /// 
        /// </summary>
        protected internal String _identifier = string.Empty;

        #endregion
    }
}
