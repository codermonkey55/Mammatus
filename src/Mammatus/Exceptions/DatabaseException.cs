using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Mammatus.Exceptions
{
    [Serializable]
    public class DatabaseException : CoreException
    {
        public DatabaseException()
        {
        }

        public DatabaseException(string message)
        : base(message)
        {
        }

        public DatabaseException(string message, Exception ex)
        : base(message, ex)
        {
        }

        protected DatabaseException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}