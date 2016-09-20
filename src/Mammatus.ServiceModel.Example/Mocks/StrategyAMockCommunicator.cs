using Mammatus.ServiceModel.Model;

namespace Mammatus.ServiceModel.Example.Mocks.Communicators
{
    public class StrategyAMockCommunicator : ICommunicator
    {
        #region ICommunicator Members

        public object InvokeOperation(object[] parameters)
        {
            return new StrategyAResponse
            {
                Field = ((StrategyARequest)parameters[0]).Field + 1
            };
        }

        #endregion
    }

    public class StrategyARequest
    {
        public int Field { get; set; }
    }

    public class StrategyAResponse
    {
        public int Field { get; set; }
    }
}