using asp.Microservice.Application.Database;
using asp.Microservice.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace asp.Microservice.Infrastructure.Config;

public static class DatabaseConfigExtensions
{
    public static IServiceCollection AddDatabaseContext(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services
            .AddDbContext<DatabaseContext>(options =>
            {
                var connectionString = GetConnectionString(configuration);
                options.UseNpgsql(connectionString);

                // https://github.com/zzzprojects/EntityFramework-Extensions/issues/441#issue-1014382709
                // https://stackoverflow.com/a/70304966
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

                options.ConfigureWarnings(w =>
                {
                    if (!environment.IsDevelopment())
                    {
                        // https://www.thinktecture.com/en/entity-framework-core/cartesian-explosion-problem-in-3-1/
                        // https://docs.microsoft.com/en-us/ef/core/querying/single-split-queries
                        w.Ignore(RelationalEventId.MultipleCollectionIncludeWarning);
                    }
                });
            })
            .AddScoped<IDatabaseContext>(x => x.GetRequiredService<DatabaseContext>());

        return services;
    }

    private static string GetConnectionString(
        IConfiguration configuration)
        => Environment.GetEnvironmentVariable("ConnectionStrings__Database")
           ?? configuration.GetConnectionString("Database")
           ?? throw new InvalidOperationException("Connection string not found");
}