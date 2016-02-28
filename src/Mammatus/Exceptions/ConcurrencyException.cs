using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Mammatus.Guid.Extensions;

namespace Mammatus.Exceptions
{
    [Serializable]
    public class ConcurrencyException : CoreException
    {
        private readonly System.Guid _UniqueId = GuidExtensions.NewCombGuid();

        public ConcurrencyException()
        {
        }

        public ConcurrencyException(string message)
        : base(message)
        {
        }

        public ConcurrencyException(string message, Exception ex)
        : base(message, ex)
        {
        }

        protected ConcurrencyException(SerializationInfo info, StreamingContext context)
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