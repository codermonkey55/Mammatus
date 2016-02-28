using System.Collections.Generic;
using System.Reflection;

namespace Mammatus.Domain.Core
{
    using System;

    [Serializable]
    public abstract class ValueObject
    {
        protected BindingFlags reflectingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private const int HASH_MULTIPLIER = 31;

        public static bool operator !=(ValueObject x, ValueObject y)
        {
            return !Equals(x, y);
        }

        public static bool operator ==(ValueObject x, ValueObject y)
        {
            return Equals(x, y);
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            // Type comparison
            if (GetType() != other.GetType())
            {
                return false;
            }

            foreach (FieldInfo field in GetType().GetFields(this.reflectingFlags))
            {
                object value1 = field.GetValue(other);
                object value2 = field.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                    {
                        return false;
                    }
                }
                else if (!value1.Equals(value2))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            // It's possible for two objects to return the same hash code based on
            // identically valued properties, even if they're of two different types,
            // so we include the object's type in the hash calculation
            int hashCode = GetType().GetHashCode();

            foreach (FieldInfo field in GetType().GetFields(this.reflectingFlags))
            {
                object value = field.GetValue(this);

                if (value != null) unchecked
                {
                    hashCode = hashCode * HASH_MULTIPLIER + value.GetHashCode();
                }
            }

            return hashCode;
        }
    }
}