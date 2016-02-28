using System;
using System.Web;
using Mammatus.Core.State;

namespace Mammatus.Web.State
{
    public interface IHttpContextProvider : IContextProvider
    {
        HttpContextBase HttpContext { get; }
    }
}
