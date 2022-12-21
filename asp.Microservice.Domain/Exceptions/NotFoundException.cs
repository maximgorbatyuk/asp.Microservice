namespace asp.Microservice.Domain.Exceptions;

public class NotFoundException : Exception
{
    public string Title { get; }

    public NotFoundException(
        string message)
        : this("Not found", message)
    {
    }

    public NotFoundException(
        string title,
        string message)
        : base(message)
    {
        Title = title;
    }
}