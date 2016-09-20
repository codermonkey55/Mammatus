using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Mammatus.ServiceModel.Contracts.Data;
using Mammatus.ServiceModel.Example.Mocks.Communicators;
using Mammatus.ServiceModel.Model;

namespace Mammatus.ServiceModel.Example
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ProxyService : IProxyService
    {

        ICommunicator OperationACommunicator { get; set; }
        IBusinessRules RequestRules { get; set; }
        ITranslator RequestTranslator { get; set; }
        ITranslator ResponseTranslator { get; set; }

        #region IProxyService Members

        public Response ExecuteProxyOperationA(Request value)
        {
            ExecuteRules(value, "ExecuteProxyOperationA");
            StrategyARequest request = RequestTranslator.Translate<Request, StrategyARequest>(value);

            var response = (StrategyAResponse)OperationACommunicator.InvokeOperation(new List<object> {
                    request
            }.ToArray());

            return ResponseTranslator.Translate<StrategyAResponse, Response>(response);
        }

        public Response ExecuteProxyOperationB(Request value)
        {
            ExecuteRules(value, "ExecuteProxyOperationB");
            StrategyBRequest request = RequestTranslator.Translate<Request, StrategyBRequest>(value);

            var response = (StrategyBResponse)OperationACommunicator.InvokeOperation(new List<object> {
                    request
            }.ToArray());

            return ResponseTranslator.Translate<StrategyBResponse, Response>(response);
        }

        #endregion

        private void ExecuteRules(Request value, string operationName)
        {
            List<string> ruleResult = RequestRules.Execute(value).ToList();
            if (ruleResult.Count == 0) return;

            List<string> operation = new List<string>();
            List<string> problemType = new List<string>();

            foreach (string violation in ruleResult)
            {
                operation.Add(operationName + "-ExecuteRules");
                problemType.Add(violation);
            }

            throw new FaultException<RulesFault>(new RulesFault
            {
                Operation = operation.ToArray(),
                ProblemType = problemType.ToArray()
            });
        }
    }
}
