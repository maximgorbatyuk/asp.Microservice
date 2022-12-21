namespace asp.Microservice.Domain.Entities;

public class User : EntityBase
{
    protected User()
    {
    }

    public User(
        string email,
        string firstName,
        string lastName)
    {
        Email = email?.Trim() ?? throw new ArgumentNullException(nameof(email));
        FirstName = firstName?.Trim() ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName?.Trim() ?? throw new ArgumentNullException(nameof(lastName));
    }

    public Guid Id { get; protected set; }

    public string? Email { get; protected set; }

    public string? FirstName { get; protected set; }

    public string? LastName { get; protected set; }
}