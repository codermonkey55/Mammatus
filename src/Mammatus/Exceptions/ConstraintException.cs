using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Mammatus.Exceptions
{
    /// <summary>
    /// Constraint Exception.
    /// </summary>
    [Serializable]
    public class ConstraintException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintException"/> class.
        /// </summary>
        public ConstraintException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ConstraintException(string message)
        : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        public ConstraintException(string message, Exception ex)
        : base(message, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected ConstraintException(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}