using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace asp.Microservice.Infrastructure.Health;

public record HealthCheckResponseDataItem
{
    public string? Key { get; init; }

    public string? Description { get; init; }

    public HealthStatus Status { get; init; }

    public TimeSpan Duration { get; init; }

    public string? Error { get; init; }

    public IReadOnlyDictionary<string, object>? Data { get; init; }
}