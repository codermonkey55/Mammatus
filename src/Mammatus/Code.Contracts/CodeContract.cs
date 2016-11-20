using Mammatus.Code.Contracts.Exceptions;
using Mammatus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Mammatus.Code.Contracts
{
    public static class CodeContract
    {
        public static bool EnableExceptions { get; set; }

        public static void Require(bool assertion, string message)
        {
            if (EnableExceptions)
            {
                if (!assertion) throw new PreconditionException(message);
            }
            else
            {
                Trace.Assert(assertion, "Precondition: " + message);
            }
        }

        public static void Require(bool assertion, string message, Exception inner)
        {
            if (EnableExceptions)
            {
                if (!assertion) throw new PreconditionException(message, inner);
            }
            else
            {
                Trace.Assert(assertion, "Precondition: " + message);
            }
        }

        public static void Require(bool assertion)
        {
            if (EnableExceptions)
            {
                if (!assertion) throw new PreconditionException("Precondition failed.");
            }
            else
            {
                Trace.Assert(assertion, "Precondition failed.");
            }
        }


        public static void Ensure(bool assertion, string message)
        {
            if (EnableExceptions)
            {
                if (!assertion) throw new PostconditionException(message);
            }
            else
            {
                Trace.Assert(assertion, "Postcondition: " + message);
            }
        }

        public static void Ensure(bool assertion, string message, Exception inner)
        {
            if (EnableExceptions)
            {
                if (!assertion) throw new PostconditionException(message, inner);
            }
            else
            {
                Trace.Assert(assertion, "Postcondition: " + message);
            }
        }

        public static void Ensure(bool assertion)
        {
            if (EnableExceptions)
            {
                if (!assertion) throw new PostconditionException("Postcondition failed.");
            }
            else
            {
                Trace.Assert(assertion, "Postcondition failed.");
            }
        }

        public static void Invariant(bool assertion, string message)
        {
            if (EnableExceptions)
            {
                if (!assertion) throw new InvariantException(message);
            }
            else
            {
                Trace.Assert(assertion, "Invariant: " + message);
            }
        }

        public static void Invariant(bool assertion, string message, Exception inner)
        {
            if (EnableExceptions)
            {
                if (!assertion) throw new InvariantException(message, inner);
            }
            else
            {
                Trace.Assert(assertion, "Invariant: " + message);
            }
        }

        public static void Invariant(bool assertion)
        {
            if (EnableExceptions)
            {
                if (!assertion) throw new InvariantException("Invariant failed.");
            }
            else
            {
                Trace.Assert(assertion, "Invariant failed.");
            }
        }

        public static void Assert(bool assertion, string message)
        {
            if (EnableExceptions)
            {
                if (!assertion) throw new AssertionException(message);
            }
            else
            {
                Trace.Assert(assertion, "Assertion: " + message);
            }
        }

        public static void Assert(bool assertion, string message, Exception inner)
        {
            if (EnableExceptions)
            {
                if (!assertion) throw new AssertionException(message, inner);
            }
            else
            {
                Trace.Assert(assertion, "Assertion: " + message);
            }
        }

        public static void Assert(bool assertion)
        {
            if (EnableExceptions)
            {
                if (!assertion) throw new AssertionException("Assertion failed.");
            }
            else
            {
                Trace.Assert(assertion, "Assertion failed.");
            }
        }

        [DebuggerStepThrough]
        public static void IsNotNull(object value, string name)
        {
            if (value == null) throw new ArgumentNullException(name);
        }

        [DebuggerStepThrough]
        public static void IsOfType(object argument, System.Type baseType, string argumentName)
        {
            IsNotNull(argument, argumentName);

            IsNotNull(baseType, "baseType");

            if (!baseType.IsInstanceOfType(argument))
                throw new ArgumentException("{0} is not of type {1}.".FormatWith(argument, baseType), argumentName);
        }

        public static void ElementCountInRange<T>(IEnumerable<T> enumerable, int min, int max, string argumentName)
        {
            IsNotNull(enumerable, argumentName);
            int count = enumerable.Count();
            if (count < min || count > max)
                throw new ArgumentOutOfRangeException("the number of items <{0}> is not in the expected range min <{1}> max <{2}>".FormatWith(count, min, max), argumentName);
        }

        public static void IsNotNullOrEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argument))
                throw new ArgumentNullException(argumentName, "string is null or empty");
        }

        public static void ValidateCollectionParameter(ref object[] param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }
            if (param.Length < 1)
            {
                throw new ArgumentException($"The array parameter '{paramName}' should not be empty.");
            }
            Hashtable hashtable = new Hashtable(param.Length);
            for (int i = param.Length - 1; i >= 0; i--)
            {
                if (hashtable.Contains(param[i]))
                {
                    throw new ArgumentException($"The array '{paramName}' should not contain duplicate values.");
                }
                hashtable.Add(param[i], param[i]);
            }
        }
    }
}
