using asp.Microservice.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace asp.Microservice.Infrastructure.HostedServices;

public class AppInitializeService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AppInitializeService> _logger;

    public AppInitializeService(
        IServiceScopeFactory scopeFactory,
        ILogger<AppInitializeService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task StartAsync(
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();

        await MigrateAsync(context, cancellationToken);
    }

    public Task StopAsync(
        CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task MigrateAsync(
        DbContext context,
        CancellationToken cancellationToken)
    {
        if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
        {
            return;
        }

        var cs = context.Database.GetConnectionString();

        if (string.IsNullOrEmpty(cs))
        {
            throw new InvalidOperationException("Connection string is empty");
        }

        var pendingMigrations = (await context.Database.GetPendingMigrationsAsync(cancellationToken)).ToArray();
        if (pendingMigrations.Any())
        {
            await context.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database migrated with {PendingMigrationsLength} migrations", pendingMigrations.Length);
        }
    }
}