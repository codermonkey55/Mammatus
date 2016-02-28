
namespace Mammatus.Validation
{
    using System;

    public class ValidationError
    {
        public ValidationError(string message, string property)
        {
            Guard.Against<ArgumentNullException>(
                                                string.IsNullOrEmpty(message),
                                                "Please provide a valid non null string as the validation error message");
            Guard.Against<ArgumentNullException>(
                                                string.IsNullOrEmpty(property),
                                                "Please provide a valid non null string as the validation property name");

            this.EntityType = typeof(void); // Avoid make this.EntityType == null as to not breaking existing code.
            this.Message = message;
            this.PropertyName = property;
        }

        public ValidationError(Type entityType, string message, string property)
        {
            Guard.Against<ArgumentNullException>(
                                                entityType == null,
                                                "Please provide a valid non null Type as the validated Entity");
            
            Guard.Against<ArgumentNullException>(
                                                string.IsNullOrEmpty("message"),
                                                "Please provide a valid non null string as the validation error message");

            Guard.Against<ArgumentNullException>(
                                                string.IsNullOrEmpty("property"),
                                                "Please provide a valid non null string as the validation property name");
            this.EntityType = entityType;
            this.Message = message;
            this.PropertyName = property;
        }

        public ValidationError(string property, Exception ex)
        : this(ex.Message, property)
        {
        }

        public Type EntityType
        {
            get;
            private set;
        }

        public string Message
        {
            get;
            private set;
        }

        public string PropertyName
        {
            get;
            private set;
        }

        public static bool operator !=(ValidationError left, ValidationError right)
        {
            return !left.Equals(right);
        }

        public static bool operator ==(ValidationError left, ValidationError right)
        {
            return left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(ValidationError))
            {
                return false;
            }

            return Equals((ValidationError)obj);
        }

        public bool Equals(ValidationError obj)
        {
            return Equals(obj.EntityType, this.EntityType)
                   && Equals(obj.PropertyName, this.PropertyName)
                   && Equals(obj.Message, this.Message);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Message.GetHashCode() * 397) ^ this.PropertyName.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("({0}::{1}) - {2}", this.EntityType, this.PropertyName, this.Message);
        }
    }
}