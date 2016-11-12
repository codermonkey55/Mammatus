using System;

namespace Mammatus.Library.Reflection.Emitter
{
    /// <summary>
    /// A wrapper for value type.  Must be used in order for Mammatus.Library.Reflection to
    /// work with value type such as struct.
    /// </summary>
    internal class ValueTypeHolder
    {
        /// <summary>
        /// Creates a wrapper for <paramref name="value"/> value type.  The wrapper
        /// can then be used with Mammatus.Library.Reflection.
        /// </summary>
        /// <param name="value">The value type to be wrapped.
        /// Must be a derivative of <code>ValueType</code>.</param>
        public ValueTypeHolder(object value)
        {
            Value = (ValueType)value;
        }

        /// <summary>
        /// The actual struct wrapped by this instance.
        /// </summary>
        public ValueType Value { get; set; }
    }
}