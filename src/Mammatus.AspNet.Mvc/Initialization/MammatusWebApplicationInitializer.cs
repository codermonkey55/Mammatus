using System;

namespace Mammatus.AspNet.Mvc.Initialization
{
    public interface IMammatusWebApplicationInitializationInvoker
    {
        void Begin_Initialization(Action<IMammatusWebApplicationInitializer> applicationInitializer);
    }

    public interface IMammatusWebApplicationInitializer
    {
        IMammatusWebApplicationInitializer EnableMammutusExceptionHandling();

        IPostInitializationContext Initialize();
    }

    public sealed class MammatusWebApplicationInitializer : IMammatusWebApplicationInitializer, IMammatusWebApplicationInitializationInvoker
    {
        private readonly IWebApplicationInitializerExecutionContext _executionContext;
        public MammatusWebApplicationInitializer()
        {
            _executionContext = new WebApplicationInitializerExecutionContext();
        }
        public void Begin_Initialization(Action<IMammatusWebApplicationInitializer> applicationInitializer)
        {
            applicationInitializer.Invoke(this);
        }

        public IMammatusWebApplicationInitializer EnableMammutusExceptionHandling()
        {
            _executionContext.MammatusExceptionHandlingEnabled = true;

            return this;
        }

        public IPostInitializationContext Initialize()
        {
            return new PostWebApplicationInitializationContext();
        }
    }
}
