namespace LSTC.CheeseShop.Api.HealthChecks
{

    using Microsoft.Extensions.Diagnostics.HealthChecks;


    public class CustomHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var rand = new Random();
            await Task.Delay(rand.Next(500, 2500));

            // Custom health check logic
            bool isHealthy = rand.Next(10) > 0;

            return isHealthy
                ? HealthCheckResult.Healthy("The service is healthy.")
                : HealthCheckResult.Unhealthy("The service is not healthy.");
        }
    }
}