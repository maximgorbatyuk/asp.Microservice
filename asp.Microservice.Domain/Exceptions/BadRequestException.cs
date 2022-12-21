namespace asp.Microservice.Domain.Exceptions;

public class BadRequestException : InvalidOperationException
{
    public BadRequestException(
        string message)
        : base(message)
    {
    }
}