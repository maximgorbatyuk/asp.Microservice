using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace asp.Microservice.Infrastructure.Health;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddAppHealthChecks(
        this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddCheck<PostgreDatabaseHealthCheck>("Database");

        return services;
    }
}