using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Mammatus.Exceptions
{
    [Serializable]
    public class DependencyException : CoreException
    {
        public DependencyException()
        {
        }

        public DependencyException(string message)
        : base(message)
        {
        }

        public DependencyException(string message, Exception ex)
        : base(message, ex)
        {
        }

        protected DependencyException(SerializationInfo info, StreamingContext context)
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