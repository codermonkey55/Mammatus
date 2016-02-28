using System.ServiceModel.Activation;
using Mammatus.Core.Application;
using Mammatus.Core.State;

namespace Mammatus.ServiceModel.State
{
    public class OperationContextProvider : ObjectBase<OperationContextProvider>, IOperationContextProvider
    {
        private IOperationContextWrapper _operationContextWrapper;

        public bool IsWebApplication
        {
            get { return false; }
        }

        public bool IsWcfApplication
        {
            get { return true; }
        }

        public bool IsAspNetCompatEnabled
        {
            get
            {
                if (!IsWcfApplication)
                    return false;
                var aspnetCompat = this.OperationContext.Host
                    .Description
                    .Behaviors
                    .Find<AspNetCompatibilityRequirementsAttribute>();

                return (aspnetCompat != null &&
                        (aspnetCompat.RequirementsMode == AspNetCompatibilityRequirementsMode.Allowed ||
                         aspnetCompat.RequirementsMode == AspNetCompatibilityRequirementsMode.Required) && IsWebApplication);
            }
        }

        public OperationContextProvider()
        {
            bool isCurrentOperationContextActive = System.ServiceModel.OperationContext.Current == null;

            _operationContextWrapper = isCurrentOperationContextActive ? OperationContextWrapper.Create(System.ServiceModel.OperationContext.Current) : null;
        }

        public OperationContextProvider(IOperationContextWrapper operationContextWrapper)
        {
            _operationContextWrapper = operationContextWrapper;
        }

        public virtual IOperationContextWrapper OperationContext
        {
            get
            {
                return _operationContextWrapper;
            }
        }
    }
}