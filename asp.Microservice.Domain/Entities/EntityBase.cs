namespace asp.Microservice.Domain.Entities;

public abstract class EntityBase : IDomainEntity
{
    public DateTime CreatedAt { get; protected set; }

    public DateTime UpdatedAt { get; protected set; }

    public void OnCreate(
        DateTime now)
    {
        CreatedAt = UpdatedAt = now;
    }

    public void OnUpdate(
        DateTime now)
    {
        UpdatedAt = now;
    }
}