using System.Diagnostics;
using System.Threading;
using System.Web;

namespace Mammatus.Helpers
{
    using System;

    public static class SystemHelper
    {
        public static string GetPath(string virtualPath) => HttpContext.Current.Server.MapPath(virtualPath);

        public static string GetMethodName(int level) => new StackTrace().GetFrame(level).GetMethod().Name;

        public static string NewGuid => Guid.NewGuid().ToString();

        public static string NewLine => Environment.NewLine;

        public static AppDomain CurrentAppDomain => Thread.GetDomain();
    }
}
