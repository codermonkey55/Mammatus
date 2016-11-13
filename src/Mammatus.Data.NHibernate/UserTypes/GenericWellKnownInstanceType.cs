﻿using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Mammatus.Data.NHibernate.UserTypes
{
    /// <summary>
    /// http://code.google.com/p/unhaddins/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TId">The type of the id.</typeparam>
    [Serializable]
    public abstract class GenericWellKnownInstanceType<T, TId> : IUserType
        where T : class
    {
        private readonly Func<T, TId, bool> findPredicate;
        private readonly Func<T, TId> idGetter;
        private readonly IEnumerable<T> repository;

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="repository">The collection that represent a in-memory repository.</param>
        /// <param name="findPredicate">The predicate an instance by the persisted value.</param>
        /// <param name="idGetter">The getter of the persisted value.</param>
        protected GenericWellKnownInstanceType(
            IEnumerable<T> repository,
            Func<T,
            TId,
            bool> findPredicate,
            Func<T, TId> idGetter)
        {
            this.repository = repository;
            this.findPredicate = findPredicate;
            this.idGetter = idGetter;
        }

        public bool IsMutable
        {
            get
            {
                return false;
            }
        }

        public Type ReturnedType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// The SQL types for the columns mapped by this type.
        /// </summary>
        public abstract SqlType[] SqlTypes
        {
            get;
        }

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object DeepCopy(object value)
        {
            return value;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public new bool Equals(object x, object y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(null, x) || ReferenceEquals(null, y))
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode(object x)
        {
            return (x == null) ? 0 : x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            int index0 = rs.GetOrdinal(names[0]);
            if (rs.IsDBNull(index0))
            {
                return null;
            }

            var value = (TId)rs.GetValue(index0);
            return this.repository.FirstOrDefault(x => this.findPredicate(x, value));
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                ((IDbDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
            }
            else
            {
                ((IDbDataParameter)cmd.Parameters[index]).Value = this.idGetter((T)value);
            }
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }
    }
}