using asp.Microservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace asp.Microservice.Infrastructure.Database.Config;

public class UsersConfig : IEntityTypeConfiguration<User>
{
    public void Configure(
        EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder
            .HasIndex(x => x.Email)
            .IsUnique();

        builder
            .Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(300);

        builder
            .Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(300);
    }
}