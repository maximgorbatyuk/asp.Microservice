using asp.Microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace asp.Microservice.Application.Database;

public interface IDatabaseContext
{
    public DbSet<User> Users { get; }

    int SaveChanges();

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken);
}