using System;
using System.Linq;
using System.Collections.Generic;

namespace Mammatus.Code.Contracts
{
    [Serializable]
    public class CodeContractException : ApplicationException
    {
        protected CodeContractException() { }
        protected CodeContractException(string message) : base(message) { }
        protected CodeContractException(string message, Exception inner) : base(message, inner) { }
    }
}