using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Company.Function
{
    public class ApplicationInsightTelemetry : ITelemetry
    {
        public TelemetryClient ApplicationInsightsClient { get; private set; }

        public void Dependency(Dependency dependency)
        {
            ApplicationInsightsClient.TrackDependency(new DependencyTelemetry
            {
                Name = dependency.Name,
                Target = dependency.Request.RequestUri.ToString(),
                Data = dependency.Response.Content.ToString(),
                Success = dependency.Response.StatusCode.Equals(HttpStatusCode.OK) || dependency.Response.StatusCode.Equals(HttpStatusCode.Accepted),
                ResultCode = dependency.Response.StatusCode.ToString()
            });
        }

        public void Error(Error error)
        {
            ApplicationInsightsClient.TrackException(new ExceptionTelemetry(error.exception));
        }

        public void Event(Event telemetryEvent)
        {
            var EventTelemetry = new EventTelemetry
            {
                Name = telemetryEvent.Name
            };
            foreach (KeyValuePair<string, string> additionalEvent in telemetryEvent.Additional)
            {
                EventTelemetry.Properties.Add(additionalEvent);
            }
            ApplicationInsightsClient.TrackEvent(EventTelemetry);
        }

        public static ITelemetry Create(TelemetryClient client)
        {
            return new ApplicationInsightTelemetry
            {
                ApplicationInsightsClient = client
            };
        }

        public void SetUserContext(string userId)
        {
            ApplicationInsightsClient.Context.User.Id = userId;
        }

        public void SetOperationContext(string executionId)
        {
            ApplicationInsightsClient.Context.Operation.Id = executionId;
        }

        public IOperationHolder<RequestTelemetry> StartOperation(Activity requestActivity)
        {
            return ApplicationInsightsClient.StartOperation<RequestTelemetry>(requestActivity);
        }
        public void StopOperation(IOperationHolder<RequestTelemetry> requestOperation)
        {
            ApplicationInsightsClient.StopOperation(requestOperation);
        }
    }
}