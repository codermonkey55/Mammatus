using System.Runtime.Remoting.Messaging;

namespace Mammatus.Core.State
{
    /// <summary>
    /// Context State Helper
    /// </summary>
    public static class ContextState
    {
        /// <summary>
        /// Gets an item by the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the item to get</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// An item of type T
        /// </returns>
        public static T Get<T>(string key)
        {
            object value = CallContext.GetData(key);
            if (value != null)
            {
                return (T)value;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// Stores the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void Store(string key, object value)
        {
            CallContext.SetData(key, value);
        }
    }
}
