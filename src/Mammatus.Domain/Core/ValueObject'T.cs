using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Mammatus.Domain.Core
{
    using System;

    [Serializable]
    public abstract class ValueObject<T> : IEquatable<T>
    where T : ValueObject<T>
    {
        protected BindingFlags reflectingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        private const int HASH_MULTIPLIER = 31;

        public static bool operator !=(ValueObject<T> x, ValueObject<T> y)
        {
            return !Equals(x, y);
        }

        public static bool operator ==(ValueObject<T> x, ValueObject<T> y)
        {
            return Equals(x, y);
        }

        public virtual bool Equals(T other)
        {
            if (other == null)
            {
                return false;
            }

            Type t = GetType();
            Type otherType = other.GetType();

            if (t != otherType)
            {
                return false;
            }

            foreach (FieldInfo field in this.GetFields(this))
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

        public override bool Equals(object obj)
        {
            var other = obj as T;

            if (other == null)
            {
                return false;
            }

            return Equals(other);
        }

        public override int GetHashCode()
        {
            // It's possible for two objects to return the same hash code based on
            // identically valued properties, even if they're of two different types,
            // so we include the object's type in the hash calculation
            int hashCode = GetType().GetHashCode();

            foreach (FieldInfo field in this.GetFields(this))
            {
                object value = field.GetValue(this);

                if (value != null) unchecked
                    {
                        hashCode = hashCode * HASH_MULTIPLIER + value.GetHashCode();
                    }
            }

            return hashCode;
        }

        private IEnumerable<FieldInfo> GetFields(object obj)
        {
            Type t = obj.GetType();
            var fields = new List<FieldInfo>();

            while (t != typeof(object))
            {
                FieldInfo[] tmp = t.GetFields(this.reflectingFlags);
                fields.AddRange(tmp);
                t = t.BaseType;
            }

            return fields;
        }
    }
}