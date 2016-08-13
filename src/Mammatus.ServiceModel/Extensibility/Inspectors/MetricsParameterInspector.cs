
using System.Diagnostics;

namespace Mammatus.ServiceModel.Extensibility.Inspectors
{
    public sealed class MetricsParameterInspector : ParameterInspectorBase
    {
        private readonly Stopwatch _operationTimer;

        public MetricsParameterInspector()
        {
            _operationTimer = new Stopwatch();
        }

        public override void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            _operationTimer.Stop();

            base.AfterCall(operationName, outputs, returnValue, correlationState);

            _operationTimer.Reset();
        }

        public override object BeforeCall(string operationName, object[] inputs)
        {
            _operationTimer.Start();

            return base.BeforeCall(operationName, inputs);
        }
    }
}
