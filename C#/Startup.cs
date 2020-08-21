using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Company.Function.Startup))]

namespace Company.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<TelemetryClient>(services =>
            {
                IConfiguration config = services.GetRequiredService<IConfiguration>();
                var telemetryConfig = new TelemetryConfiguration(config["APPINSIGHTS_INSTRUMENTATIONKEY"]);
                return new TelemetryClient(telemetryConfig);
            }).AddSingleton<ITelemetry>(services =>
            {
                IConfiguration config = services.GetRequiredService<IConfiguration>();
                var telemetryConfig = new TelemetryConfiguration(config["APPINSIGHTS_INSTRUMENTATIONKEY"])
                {
                    ConnectionString = $"InstrumentationKey={config["APPINSIGHTS_INSTRUMENTATIONKEY"]}"
                };
                TelemetryClient client = new TelemetryClient(telemetryConfig);
                return ApplicationInsightTelemetry.Create(client);
            });
        }
    }
}