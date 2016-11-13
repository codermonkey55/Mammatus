
namespace Mammatus.Domain.Core
{
    using System;

    [Serializable]
    public abstract class BaseEntity<TEntity, TKey> : ValidatableObject<TEntity>, IEquatable<TEntity>, IEntity<TKey>
        where TEntity : BaseEntity<TEntity, TKey>
        where TKey : struct, IEquatable<TKey>
    {
        private const int HASH_MULTIPLIER = 31;

        private int? cachedHashcode;

        public virtual TKey Id
        {
            get;
            protected set;
        }

        public virtual string Version
        {
            get;
            protected set;
        }

        public virtual bool Equals(TEntity other)
        {
            return base.Equals(other);
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as BaseEntity<TEntity, TKey>;

            if (object.ReferenceEquals(this, compareTo))
            {
                return true;
            }

            if (compareTo == null || compareTo is TEntity == false)
            {
                return false;
            }

            if (this.IsTransient())
            {
                return false;
            }

            return HasSameNonDefaultIdAs(compareTo);
        }

        public override int GetHashCode()
        {
            // Once we have a hash code we'll never change it
            if (this.cachedHashcode.HasValue)
            {
                return this.cachedHashcode.Value;
            }

            if (IsTransient())
            {
                this.cachedHashcode = base.GetHashCode();
            }
            else
            {
                unchecked
                {
                    // It's possible for two objects to return the same hash code based on
                    // identically valued properties, even if they're of two different types,
                    // so we include the object's type in the hash calculation
                    int hashCode = this.GetType().GetHashCode();
                    this.cachedHashcode = (hashCode * HASH_MULTIPLIER) ^ this.Id.GetHashCode();
                }
            }

            return cachedHashcode.Value;
        }

        public virtual bool IsTransient()
        {
            return this.Id.Equals(default(TKey));
        }

        protected virtual Type TypeWithoutProxy()
        {
            return this.GetType();
        }

        private bool HasSameNonDefaultIdAs(BaseEntity<TEntity, TKey> compareTo)
        {
            return !this.IsTransient() &&
                   !compareTo.IsTransient() &&
                   this.Id.Equals(compareTo.Id);
        }
    }
}