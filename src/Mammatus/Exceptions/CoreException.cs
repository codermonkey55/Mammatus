using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Mammatus.Extensions;

namespace Mammatus.Exceptions
{
    [Serializable]
    public abstract class CoreException : Exception
    {
        private readonly System.Guid _UniqueId = GuidExtensions.NewCombGuid();

        protected CoreException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        protected CoreException()
        {
        }

        protected CoreException(string message)
            : base(message)
        {
        }

        protected CoreException(string message, Exception ex)
            : base(message, ex)
        {
        }

        public System.Guid UniqueId
        {
            get
            {
                return _UniqueId;
            }
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}