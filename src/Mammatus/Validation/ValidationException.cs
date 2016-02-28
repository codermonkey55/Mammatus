using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Mammatus.Exceptions;

namespace Mammatus.Validation
{
    using System;

    [Serializable]
    public class ValidationException : CoreException
    {
        public ValidationException(Type entityType, string message)
        : base(message)
        {
            ValidationErrors = new ValidationError[] { };
            EntityType = entityType;
        }

        public ValidationException(Type entityType, string message, IEnumerable<ValidationError> errors)
        : base(message)
        {
            ValidationErrors = errors;
            EntityType = entityType;
        }

        public ValidationException(Type entityType, IEnumerable<ValidationError> errors)
        : base(string.Format(CultureInfo.InvariantCulture, "Entity {0} is not valid.", entityType.Name))
        {
            ValidationErrors = errors;
            EntityType = entityType;
        }

        public Type EntityType
        {
            get;
            protected set;
        }

        public IEnumerable<ValidationError> ValidationErrors
        {
            get;
            private set;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}