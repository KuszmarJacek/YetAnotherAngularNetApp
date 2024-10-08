using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

namespace YetAnotherAngularNetApp.Server
{
    public class CustomHealthCheckOptions : HealthCheckOptions
    {
        public CustomHealthCheckOptions() : base() 
        {
            // Create a json serializer which separates every healthcheck passed to it into their own individual results instead of grouping them together
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            ResponseWriter = async (context, result) =>
            {
                context.Response.ContentType = "application/json";
                /*
                 * Setting hardcoded 200 OK seems hacky, but each invdividual check can return 200 for a healthy result, and 503 if a check is unhealthy.
                 * Since we want to serialize a bunch of checks we return 200 every time and simply return the results of each check without 503
                 * due to potentially breaking UIs.
                 */
                context.Response.StatusCode = 200;
                var responseBody = JsonSerializer.Serialize(new
                {
                    checks = result.Entries.Select(e => new
                    {
                        name = e.Key,
                        responseTime = e.Value.Duration.TotalMilliseconds,
                        status = e.Value.Status.ToString(),
                        description = e.Value.Description
                    }),
                    // boolean sum of all checks
                    totalStatus = result.Status.ToString(),
                    // duration of all checks
                    totalResponseTime = result.TotalDuration.TotalMilliseconds,
                }, jsonSerializerOptions);
                await context.Response.WriteAsync(responseBody);
            };
        }
    }
}
