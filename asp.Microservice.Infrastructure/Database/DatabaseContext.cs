using asp.Microservice.Application.Database;
using asp.Microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace asp.Microservice.Infrastructure.Database;

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DatabaseContext(
        DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsForEntitiesInContext();
    }

    public override int SaveChanges(
        bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void OnBeforeSaving()
    {
        var currentDateTime = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IDomainEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.Entity.OnUpdate(currentDateTime);
                    break;

                case EntityState.Added:
                    entry.Entity.OnCreate(currentDateTime);
                    break;
            }
        }
    }
}