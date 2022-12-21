using System.Net.Mime;
using System.Text.Json;
using asp.Microservice.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace asp.Microservice.Infrastructure.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionMiddleware(
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Request handling method.
    /// </summary>
    /// <param name="context">The Microsoft.AspNetCore.Http.HttpContext for the current request.</param>
    /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
    /// <returns>A System.Threading.Tasks.Task that represents the execution of this middleware.</returns>
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
            await HandleExceptionAsync(
                context,
                ex);
        }
    }

    protected virtual string GetTitle(Exception exception) =>
        exception switch
        {
            NotFoundException nf => nf.Title,
            ValidationException => "Validation error",
            BadRequestException => "Bad request",
            ApiException => "Api call error",
            _ => "Server Error"
        };

    protected virtual int GetStatusCode(Exception exception) =>
        exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
            BadRequestException => StatusCodes.Status400BadRequest,
            ApiException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

    private async Task HandleExceptionAsync(
        HttpContext httpContext,
        Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;

        var response = new
        {
            title = GetTitle(exception),
            status = statusCode,
            detail = exception.Message,
            errors = GetValidationErrors(exception),
            trace = _environment.IsDevelopment()
                ? exception.ToString()
                : null,
        };

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static IReadOnlyDictionary<string, string[]>? GetValidationErrors(
        Exception exception)
    {
        if (exception is ValidationException validationException)
        {
            return validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());
        }

        return null;
    }
}