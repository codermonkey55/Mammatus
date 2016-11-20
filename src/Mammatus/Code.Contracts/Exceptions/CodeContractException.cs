using System;

namespace Mammatus.Code.Contracts.Exceptions
{
    [Serializable]
    public class CodeContractException : ApplicationException
    {
        protected CodeContractException() { }
        protected CodeContractException(string message) : base(message) { }
        protected CodeContractException(string message, Exception inner) : base(message, inner) { }
    }
}