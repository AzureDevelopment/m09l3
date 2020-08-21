using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

public class UserIdTelemetryInitializer : ITelemetryInitializer
{
    private readonly string _userId;

    public UserIdTelemetryInitializer(string userId)
    {
        _userId = userId;
    }

    public void Initialize(ITelemetry telemetry)
    {
        telemetry.Context.User.Id = _userId;
    }
}