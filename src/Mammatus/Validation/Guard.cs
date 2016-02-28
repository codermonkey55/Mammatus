using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammatus.Validation
{
    public sealed class Guard
    {
        private Guard()
        {

        }

        public static void Against<TException>(bool assertion, string message) where TException : Exception
        {
            if (assertion)
                throw (TException)Activator.CreateInstance(typeof(TException), message);
        }

        public static void Against<TException>(Func<bool> assertion, string message) where TException : Exception
        {
            //Execute the lambda and if it evaluates to true then throw the exception.
            if (assertion())
                throw (TException)Activator.CreateInstance(typeof(TException), message);
        }

        public static void IsNotNull(object instance, string message)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(message);
            }
        }

        public static void IsNotNullNorEmpty(string instance, string message)
        {
            if (string.IsNullOrEmpty(instance))
            {
                throw new ArgumentNullException(message);
            }
        }

        public static void IsNotNullNorEmpty<T>(IEnumerable<T> collection, string message)
        {
            if (collection == null || collection.Count() <= 0)
            {
                throw new ArgumentNullException(message);
            }
        }
    }
}
