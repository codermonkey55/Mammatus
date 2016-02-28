using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mammatus.Core.Exceptions
{
    [Serializable]
    public class ExceptionBase : Exception
    {
        public ExceptionBase()
        {

        }

        public ExceptionBase(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        public ExceptionBase(string message)
            : base(message)
        {

        }

        public ExceptionBase(string message, ExceptionBase innerException)
            : base(message, innerException)
        {

        }
    }
}
