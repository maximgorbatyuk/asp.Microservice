using asp.Microservice.Application;
using asp.Microservice.Infrastructure.Config;
using asp.Microservice.Infrastructure.Health;
using asp.Microservice.Infrastructure.HostedServices;
using asp.Microservice.Infrastructure.Middlewares;
using asp.Microservice.Infrastructure.Validation;
using MediatR;

namespace asp.Microservice;

public class Program
{
    public static void Main(
        params string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services
            .AddRouting(options => options.LowercaseUrls = true)
            .AddEndpointsApiExplorer()
            .AddHttpContextAccessor()
            .AddDatabaseContext(
                builder.Configuration,
                builder.Environment)
            .AddAutoMapper(
                typeof(Program),
                ApplicationAssembly.Type())
            .AddMediatR(
                typeof(Program),
                ApplicationAssembly.Type())
            .AddCqrsValidation()
            .AddHostedService<AppInitializeService>()
            .AddEndpointsApiExplorer()
            .SetUpFluentValidation(
                typeof(Program),
                ApplicationAssembly.Type())
            .AddTransient<LoggingMiddleware>()
            .AddTransient<ExceptionMiddleware>();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SetUpSwaggerDefinition("asp.Microservice API");
        });

        var app = builder.Build();

        app.UseMiddleware<ExceptionMiddleware>();
        app.UseMiddleware<LoggingMiddleware>();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapHealthChecks("/health", new HealthCheckJsonResponseOptions());

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}