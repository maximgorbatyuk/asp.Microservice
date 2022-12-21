namespace asp.Microservice.Domain.Entities;

public interface IDomainEntity
{
    DateTime CreatedAt { get; }

    DateTime UpdatedAt { get; }

    void OnCreate(
        DateTime now);

    void OnUpdate(
        DateTime now);
}