using System;
using System.Linq;
using System.Collections.Generic;

namespace Mammatus.Code.Contracts
{
    [Serializable]
    public class PreconditionException : CodeContractException
    {
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException() { }
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException(string message) : base(message) { }
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException(string message, Exception inner) : base(message, inner) { }
    }
}