using System.Net;

namespace asp.Microservice.Domain.Exceptions;

public class ApiException : Exception
{
    public HttpStatusCode StatusCode { get; }

    public string? Response { get; }

    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }

    public ApiException(
        string? message,
        HttpStatusCode statusCode,
        string? response,
        IReadOnlyDictionary<string, IEnumerable<string>> headers,
        Exception? innerException = null)
        : base($"{message}\nStatus: {statusCode}\nResponse: {response ?? "empty"}", innerException)
    {
        StatusCode = statusCode;
        Response = response;
        Headers = headers;
    }
}