using System;
using System.Runtime.Serialization;

namespace Mammatus.ServiceModel.Client.DynamicProxy
{
    /// <summary>
    /// This exception is thrown by the client proxy when problems occurred 
    /// with calls made to the service. This can for example be caused because 
    /// the service is no longer available or when an internal exception occurred 
    /// within the service.
    /// </summary>
    [Serializable]
    public class ConnectionProblemException : Exception
    {
        protected ConnectionProblemException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public ConnectionProblemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
