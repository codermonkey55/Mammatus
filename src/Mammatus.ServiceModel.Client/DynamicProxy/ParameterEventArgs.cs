using System;

namespace Mammatus.ServiceModel.Client.DynamicProxy
{
    /// <summary>
    /// This is the definition for a generic eventargs class with one
    /// parameter.
    /// </summary>
    public class ParameterEventArgs<T> : EventArgs
    {
        #region Constructor

        public ParameterEventArgs(T parameter)
        {
            _parameter = parameter;
        }

        #endregion

        #region Properties

        public T Parameter
        {
            get
            {
                return _parameter;
            }
        }

        #endregion

        #region Fields

        private T _parameter;

        #endregion
    }
}
