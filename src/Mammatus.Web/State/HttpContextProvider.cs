using System.Web;

namespace Mammatus.Web.State
{
    public class HttpContextProvider : IHttpContextProvider
    {
        public bool IsWebApplication
        {
            get { return true; }
        }

        public bool IsWcfApplication
        {
            get { return false; }
        }

        public bool IsAspNetCompatEnabled
        {
            get { return false; }
        }

        public virtual HttpContextBase HttpContext
        {
            get
            {
                if (System.Web.HttpContext.Current == null)
                    return null;
                return new HttpContextWrapper(System.Web.HttpContext.Current);
            }
        }
    }
}