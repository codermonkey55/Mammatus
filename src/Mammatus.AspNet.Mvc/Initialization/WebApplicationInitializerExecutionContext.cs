namespace Mammatus.AspNet.Mvc.Initialization
{
    internal interface IWebApplicationInitializerExecutionContext
    {
        bool MammatusExceptionHandlingEnabled { get; set; }
    }

    internal sealed class WebApplicationInitializerExecutionContext : IWebApplicationInitializerExecutionContext
    {
        public bool MammatusExceptionHandlingEnabled { get; set; }
    }
}
