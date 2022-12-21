using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace asp.Microservice.Infrastructure.Validation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCqrsValidation(
        this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}