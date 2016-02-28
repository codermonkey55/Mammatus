using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammatus.Core.Application
{
    public abstract class ObjectBase<TDerived>
        where TDerived : class
    {
        public static TDerived Create()
        {
            dynamic newObject = Activator.CreateInstance(typeof(TDerived), true);
            TDerived derivedTypeInstance = newObject;
            return derivedTypeInstance;
        }

        public static TDerived Create(params object[] constructorArgs)
        {
            dynamic newObject = Activator.CreateInstance(typeof(TDerived), constructorArgs);
            TDerived derivedTypeInstance = newObject;
            return derivedTypeInstance;
        }
    }
}
