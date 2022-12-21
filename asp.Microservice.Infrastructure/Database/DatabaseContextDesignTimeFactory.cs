using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace asp.Microservice.Infrastructure.Database;

public class DatabaseContextDesignTimeFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    private readonly IConfiguration _configuration;

    public DatabaseContextDesignTimeFactory()
    {
        var directory = new DirectoryInfo("../asp.Microservice");

        _configuration = new ConfigurationBuilder()
            .SetBasePath(directory.FullName)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .AddEnvironmentVariables()
            .Build();
    }

    public DatabaseContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<DatabaseContext>();
        builder.EnableDetailedErrors(true);
        builder.EnableSensitiveDataLogging(true);

        var connectionString = GetConnectionString();

        Console.WriteLine(connectionString);

        builder.UseNpgsql(connectionString);
        return new DatabaseContext(builder.Options);
    }

    private string? GetConnectionString()
        => Environment.GetEnvironmentVariable("ConnectionStrings__Database")
           ?? _configuration.GetConnectionString("Database");
}