namespace Mammatus.Library.Reflection.Caching
{
    /// <summary>
    /// An enumeration of the supported caching strategies.
    /// </summary>
    internal enum CacheStrategy
    {
        /// <summary>
        /// This value indicates that caching is disabled.
        /// </summary>
        None,
        /// <summary>
        /// This value indicates that caching is enabled, and that cached objects may be
        /// collected and released at will by the garbage collector. This is the default value.
        /// </summary>
        Temporary,
        /// <summary>
        /// This value indicates that caching is enabled, and that cached objects may not
        /// be garbage collected. The developer must manually ensure that objects are
        /// removed from the cache when they are no longer needed.
        /// </summary>
        Permanent
    }
}