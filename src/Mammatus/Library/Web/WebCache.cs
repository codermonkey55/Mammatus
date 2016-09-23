using System;
using System.Web;
using System.Web.Caching;

namespace Mammatus.Library.Web
{
    public class WebCache
    {
        public static object GetCache(string cacheKey)
        {
            Cache objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
        }

        public static void SetCache(string cacheKey, object objObject)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject);
        }

        public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }
    }
}
