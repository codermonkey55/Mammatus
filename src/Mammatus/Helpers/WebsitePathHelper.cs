using System.Text.RegularExpressions;
using System.Web;

namespace Mammatus.Helpers
{
    public static class WebSitePathHelper
    {
        public static string GetWebPath(string localPath)
        {
            string path = HttpContext.Current.Request.ApplicationPath;
            string thisPath;
            string thisLocalPath;
            if (path != "/")
            {
                thisPath = path + "/";
            }
            else
            {
                thisPath = path;
            }
            if (localPath.StartsWith("~/"))
            {
                thisLocalPath = localPath.Substring(2);
            }
            else
            {
                return localPath;
            }
            return thisPath + thisLocalPath;
        }
        public static string GetWebPath()
        {
            string path = HttpContext.Current.Request.ApplicationPath;
            string thisPath;
            if (path != "/")
            {
                thisPath = path + "/";
            }
            else
            {
                thisPath = path;
            }
            return thisPath;
        }
        public static string GetFilePath(string localPath)
        {
            if (Regex.IsMatch(localPath, @"([A-Za-z]):\\([\S]*)"))
            {
                return localPath;
            }
            else
            {
                return HttpContext.Current.Server.MapPath(localPath);
            }
        }
    }
}
