using System.Diagnostics;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Primitives;

namespace Company.Function
{
    public interface ITelemetry
    {
        void Event(Event telemetryEvent);
        void Error(Error error);
        void Dependency(Dependency dependency);
        void SetUserContext(string userId);
        void SetOperationContext(string executionId);
        IOperationHolder<RequestTelemetry> StartOperation(Activity requestActivity);
        void StopOperation(IOperationHolder<RequestTelemetry> requestOperation);
    }
}