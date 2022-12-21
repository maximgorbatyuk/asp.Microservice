using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace asp.Microservice.Infrastructure.Config;

public static class CollectionServiceExtensions
{
    public static void SetUpSwaggerDefinition(
        this SwaggerGenOptions options,
        string title)
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = title,
            Description = "Microservice API",
        });

        options.AddSecurityDefinition(
            "x-api-key", new OpenApiSecurityScheme
        {
            Description = "Provide apiKey header for API",
            In = ParameterLocation.Header,
            Name = "x-api-key",
            Type = SecuritySchemeType.ApiKey
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

        options.IncludeXmlComments(xmlFilePath);
    }

    public static IServiceCollection SetUpFluentValidation(
        this IServiceCollection services,
        params Type[] types)
    {
        services.AddFluentValidationAutoValidation();

        foreach (var type in types)
        {
            services.AddValidatorsFromAssemblyContaining(type);
        }

        ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
        {
            if (memberInfo is not null && Attribute.IsDefined(memberInfo, typeof(JsonPropertyNameAttribute)))
            {
                var jsonProperties = (JsonPropertyNameAttribute[])memberInfo.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false);
                return jsonProperties[0].Name;
            }

            return memberInfo?.Name;
        };

        return services;
    }
}