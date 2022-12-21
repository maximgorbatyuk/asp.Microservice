using System.Linq.Expressions;

namespace asp.Microservice.Application.Database;

public static class DatabaseExtensions
{
    /// <summary>
    /// The LINQ method which makes it simpler to apply Where clause to the query only if the condition equals to true.
    /// <example>
    /// .WhereIf(some_condition, x => x.Property == propertyValue)
    /// </example>
    /// </summary>
    /// <param name="query">Query.</param>
    /// <param name="condition">Boolean condition or true/false value.</param>
    /// <param name="predicate">Predicate for expression.</param>
    /// <typeparam name="TSource">Entity type.</typeparam>
    /// <returns>IQueryable.</returns>
    public static IQueryable<TSource> WhereIf<TSource>(
        this IQueryable<TSource> query,
        bool condition,
        Expression<Func<TSource, bool>> predicate)
    {
        if (condition)
        {
            query = query.Where(predicate);
        }

        return query;
    }
}