using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;

namespace YetAnotherAngularNetApp.Server
{
    public class ICMPHealthCheck : IHealthCheck
    {
        private readonly string Host;
        private readonly int HealthyRoundtripTime;

        public ICMPHealthCheck(string host, int healthyRoundtripTime)
        {
            Host = host;
            HealthyRoundtripTime = healthyRoundtripTime;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(Host);
                switch (reply.Status)
                {
                    case IPStatus.Success:
                        var message = $"ICMP to {Host} took {reply.RoundtripTime} milliseconds";
                        if (reply.RoundtripTime > HealthyRoundtripTime)
                        {
                            return HealthCheckResult.Degraded(message);
                        }
                        return HealthCheckResult.Healthy(message);
                    default:
                        var error = $"ICMP to {Host} failed: {reply.Status}";
                        return HealthCheckResult.Unhealthy(error);
                }
            }
            catch (Exception ex)
            {
                var error = $"ICMP failed {ex.Message}";
                return HealthCheckResult.Unhealthy(error);
            }
        }
    }
}
