using NHibernate;
using System;

namespace Mammatus.Data.NHibernate.Interceptors
{
    /// <summary>
    /// <see cref="http://gunnarpeipman.com/2013/07/implementing-simple-change-tracking-using-nhibernate/"/>
    /// <see cref="http://nhibernate.info/doc/nhibernate-reference/events.html"/>
    /// </summary>
    public class TrackingInterceptor : EmptyInterceptor
    {
        public override bool OnSave(object entity, object id, object[] state,
                                    string[] propertyNames,
                                    global::NHibernate.Type.IType[] types)
        {
            var time = DateTime.Now;
            var userName = string.Empty; // --> Find user name here

            var typedEntity = (Entities.Entity)entity;
            typedEntity.Created = time;
            typedEntity.CreatedBy = userName;
            typedEntity.Modified = time;
            typedEntity.ModifiedBy = userName;

            var indexOfCreated = GetIndex(propertyNames, "Created");
            var indexOfCreatedBy = GetIndex(propertyNames, "CreatedBy");
            var indexOfModified = GetIndex(propertyNames, "Modified");
            var indexOfModifiedBy = GetIndex(propertyNames, "ModifiedBy");

            state[indexOfCreated] = time;
            state[indexOfCreatedBy] = userName;
            state[indexOfModified] = time;
            state[indexOfModifiedBy] = userName;

            return base.OnSave(entity, id, state, propertyNames, types);
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState,
                                          object[] previousState, string[] propertyNames,
                                          global::NHibernate.Type.IType[] types)
        {
            var userName = string.Empty; // Find user name here

            var time = DateTime.Now;

            var indexOfCreated = GetIndex(propertyNames, "Created");
            var indexOfCreatedBy = GetIndex(propertyNames, "CreatedBy");
            var indexOfModified = GetIndex(propertyNames, "Modified");
            var indexOfModifiedBy = GetIndex(propertyNames, "ModifiedBy");

            var typedEntity = (Entities.Entity)entity;
            if (typedEntity.Created == DateTime.MinValue)
            {
                currentState[indexOfCreated] = time;
                currentState[indexOfCreatedBy] = userName;
                typedEntity.Created = time;
                typedEntity.CreatedBy = userName;
            }

            currentState[indexOfModified] = time;
            currentState[indexOfModifiedBy] = userName;
            typedEntity.Modified = time;
            typedEntity.ModifiedBy = userName;

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        private int GetIndex(string[] array, string property)
        {
            for (var i = 0; i < array.Length; i++)
                if (array[i].ToString() == property)
                    return i;

            return -1;
        }
    }
}