using System.Data.Common;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace asp.Microservice.Infrastructure.Health;

public abstract class DatabaseHealthCheckBase : IHealthCheck
{
    private const string DefaultTestQuery = "Select 1";

    protected abstract DbConnection Connection();

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken)
    {
        await using var connection = Connection();
        try
        {
            await connection.OpenAsync(cancellationToken);

            var command = connection.CreateCommand();
            command.CommandText = DefaultTestQuery;

            await command.ExecuteNonQueryAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (DbException ex)
        {
            return new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex);
        }
    }
}