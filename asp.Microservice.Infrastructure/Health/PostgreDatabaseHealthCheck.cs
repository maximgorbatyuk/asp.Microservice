using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace asp.Microservice.Infrastructure.Health;

public class PostgreDatabaseHealthCheck : DatabaseHealthCheckBase
{
    private readonly string _connectionString;

    public PostgreDatabaseHealthCheck(
        IConfiguration configuration)
    {
        _connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Database")
                            ?? configuration.GetConnectionString("Database")
                            ?? throw new InvalidOperationException("No connection string");
    }

    protected override DbConnection Connection()
        => new NpgsqlConnection(_connectionString);
}