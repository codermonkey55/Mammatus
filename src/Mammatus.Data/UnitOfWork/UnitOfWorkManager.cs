using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mammatus;
using Mammatus.Validation;

namespace Mammatus.Data.UnitOfWork
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly LinkedList<IUnitOfWorkScope> _scopes = new LinkedList<IUnitOfWorkScope>();

        private readonly IUnitOfWorkFactory _uowFactory;

        public IUnitOfWork CurrentUnitOfWork
        {
            get
            {
                return CurrentScope.UnitOfWork;
            }
        }

        public IUnitOfWorkScope CurrentScope
        {
            get
            {
                IUnitOfWorkScope scope = null;

                if (_scopes.Count == 0)
                {
                    scope = BeginScope();
                }
                else
                {
                    scope = _scopes.Last();
                }
                return scope;
            }
        }

        public UnitOfWorkManager(IUnitOfWorkFactory uowFactory)
        {
            Guard.Against<ArgumentNullException>(uowFactory == null, "The uow factory cannot be null.");


            _uowFactory = uowFactory;
        }

        public IUnitOfWorkScope BeginScope()
        {
            var uow = _uowFactory.Create();
            var scope = new UnitOfWorkScope(uow, this);
            _scopes.AddLast(scope);
            return scope;
        }

        public void EndScope(IUnitOfWorkScope scope)
        {
            Guard.Against<ArgumentNullException>(scope == null, "The unit of work scope cannot be null.");

            _scopes.Remove(scope);
            scope.Dispose();
        }

        public void Dispose()
        {
            if (_scopes.Count > 0)
            {
                foreach (var scope in _scopes.ToArray())
                    scope.Dispose();
            }
        }
    }
}
