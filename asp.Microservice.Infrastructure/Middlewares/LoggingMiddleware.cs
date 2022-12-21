using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace asp.Microservice.Infrastructure.Middlewares;

public class LoggingMiddleware : IMiddleware
{
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(
        ILogger<LoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception during web request");
            throw;
        }
    }
}