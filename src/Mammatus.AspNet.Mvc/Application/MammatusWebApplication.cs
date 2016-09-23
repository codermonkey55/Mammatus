using System;
using Mammatus.AspNet.Mvc.Initialization;

namespace Mammatus.AspNet.Mvc.Application
{
    public abstract class MammatusWebApplication : System.Web.HttpApplication
    {

        protected virtual void Configure(Action<IMammatusWebApplicationInitializationInvoker> invoker)
        {
            invoker.Invoke(new MammatusWebApplicationInitializer());
        }

        protected virtual void Application_Start(object sender, EventArgs e)
        {

        }

        protected virtual void Session_Start(object sender, EventArgs e)
        {

        }

        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected virtual void Application_Error(object sender, EventArgs e)
        {

        }

        protected virtual void Session_End(object sender, EventArgs e)
        {

        }

        protected virtual void Application_End(object sender, EventArgs e)
        {

        }
    }
}
