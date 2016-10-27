using NHibernate;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Collections;

namespace Mammatus.Data.NHibernate.Interceptors
{
    public class PreCommitInterceptor : IInterceptor
    {
        public bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            throw new System.NotImplementedException();
        }

        public bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames,
            IType[] types)
        {
            throw new System.NotImplementedException();
        }

        public bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            throw new System.NotImplementedException();
        }

        public void OnDelete(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            throw new System.NotImplementedException();
        }

        public void OnCollectionRecreate(object collection, object key)
        {
            throw new System.NotImplementedException();
        }

        public void OnCollectionRemove(object collection, object key)
        {
            throw new System.NotImplementedException();
        }

        public void OnCollectionUpdate(object collection, object key)
        {
            throw new System.NotImplementedException();
        }

        public void PreFlush(ICollection entities)
        {
            throw new System.NotImplementedException();
        }

        public void PostFlush(ICollection entities)
        {
            throw new System.NotImplementedException();
        }

        public bool? IsTransient(object entity)
        {
            throw new System.NotImplementedException();
        }

        public int[] FindDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames,
            IType[] types)
        {
            throw new System.NotImplementedException();
        }

        public object Instantiate(string entityName, EntityMode entityMode, object id)
        {
            throw new System.NotImplementedException();
        }

        public string GetEntityName(object entity)
        {
            throw new System.NotImplementedException();
        }

        public object GetEntity(string entityName, object id)
        {
            throw new System.NotImplementedException();
        }

        public void AfterTransactionBegin(ITransaction tx)
        {
            throw new System.NotImplementedException();
        }

        public void BeforeTransactionCompletion(ITransaction tx)
        {
            throw new System.NotImplementedException();
        }

        public void AfterTransactionCompletion(ITransaction tx)
        {
            throw new System.NotImplementedException();
        }

        public SqlString OnPrepareStatement(SqlString sql)
        {
            throw new System.NotImplementedException();
        }

        public void SetSession(ISession session)
        {
            throw new System.NotImplementedException();
        }
    }
}