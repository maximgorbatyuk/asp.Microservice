using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace asp.Microservice.Infrastructure.Health;

public record HealthCheckResponseData
{
    public HealthStatus Status { get; init; }

    public TimeSpan Duration { get; init; }

    public IReadOnlyCollection<HealthCheckResponseDataItem> Info { get; init; } = Array.Empty<HealthCheckResponseDataItem>();

    public IReadOnlyDictionary<string, object> GetAsData()
        => new Dictionary<string, object>()
        {
            { "status", Status.ToString() },
            { "duration", Duration.ToString() },
            {
                "info", Info.Select(x => new
                {
                    x.Key,
                    x.Status,
                })
            },
        };
}