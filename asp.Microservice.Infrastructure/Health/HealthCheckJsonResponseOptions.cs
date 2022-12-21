using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace asp.Microservice.Infrastructure.Health;

public class HealthCheckJsonResponseOptions : HealthCheckOptions
{
    internal static readonly JsonSerializerOptions JsonSerializerOptions = new ()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            new JsonStringEnumConverter(),
        }
    };

    public HealthCheckJsonResponseOptions()
    {
        ResponseWriter = WriteAsync;
    }

    public static Task WriteAsync(
        HttpContext context,
        HealthReport report)
    {
        var json = JsonSerializer.Serialize(
            new HealthCheckResponseData
            {
                Status = report.Status,
                Duration = report.TotalDuration,
                Info = report.Entries
                    .Select(e =>
                        new HealthCheckResponseDataItem
                        {
                            Key = e.Key,
                            Description = e.Value.Description,
                            Duration = e.Value.Duration,
                            Status = e.Value.Status,
                            Error = e.Value.Exception?.Message,
                            Data = e.Value.Data
                        })
                    .ToList()
            },
            JsonSerializerOptions);

        context.Response.ContentType = MediaTypeNames.Application.Json;
        return context.Response.WriteAsync(json);
    }
}