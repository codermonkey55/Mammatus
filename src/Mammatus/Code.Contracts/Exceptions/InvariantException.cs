using System;
using System.Linq;
using System.Collections.Generic;

namespace Mammatus.Code.Contracts
{
    [Serializable]
    public class InvariantException : CodeContractException
    {
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException() { }
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException(string message) : base(message) { }
        /// <summary>
        /// Invariant Exception.
        /// </summary>
        public InvariantException(string message, Exception inner) : base(message, inner) { }
    }
}