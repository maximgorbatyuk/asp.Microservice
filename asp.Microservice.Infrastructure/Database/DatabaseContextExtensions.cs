using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace asp.Microservice.Infrastructure.Database;

public static class DatabaseContextExtensions
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new ()
    {
        AllowTrailingCommas = true,
    };

    internal static void ApplyConfigurationsForEntitiesInContext(this ModelBuilder modelBuilder)
    {
        var types = modelBuilder.Model
            .GetEntityTypes()
            .Select(t => t.ClrType)
            .ToHashSet();

        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(),
            t => t.GetInterfaces()
                .Any(i => i.IsGenericType
                          && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)
                          && types.Contains(i.GenericTypeArguments[0])));
    }

    /// <summary>
    /// Applies JSON conversation mechanism to the selected prop.
    /// </summary>
    /// <param name="propertyBuilder">Property.</param>
    /// <typeparam name="T">Type of the property.</typeparam>
    /// <returns>Property builder.</returns>
    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
        where T : class
    {
        var converter = new ValueConverter<T, string>(
            v => JsonSerializer.Serialize(v, _jsonSerializerOptions),
            v => JsonSerializer.Deserialize<T>(v, _jsonSerializerOptions) !);

        var comparer = new ValueComparer<T>(
            (l, r) => JsonSerializer.Serialize(l, _jsonSerializerOptions) == JsonSerializer.Serialize(r, _jsonSerializerOptions),
            v => v == null ? 0 : JsonSerializer.Serialize(v, _jsonSerializerOptions).GetHashCode(),
            v => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(v, _jsonSerializerOptions), _jsonSerializerOptions) !);

        propertyBuilder.HasConversion(converter);
        propertyBuilder.Metadata.SetValueConverter(converter);
        propertyBuilder.Metadata.SetValueComparer(comparer);

        return propertyBuilder;
    }
}