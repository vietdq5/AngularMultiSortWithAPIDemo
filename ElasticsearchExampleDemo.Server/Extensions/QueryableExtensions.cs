using ElasticsearchExampleDemo.Server.AppSettings;
using ElasticsearchExampleDemo.Server.Controllers;
using System.Linq.Expressions;
using System.Text;

namespace ElasticsearchExampleDemo.Server.Extensions;

public static class QueryableExtensions
{
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderBy(CreateExpression<T>(propertyName));
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderByDescending(CreateExpression<T>(propertyName));
    }

    private static Expression<Func<T, object>> CreateExpression<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        Expression property = Expression.Property(parameter, propertyName);
        var conversion = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(conversion, parameter);
    }

    public static IOrderedQueryable<T> OrderByDynamic<T>(
         this IQueryable<T> query,
         IEnumerable<SortColumn> sortCriteria)
    {
        if (sortCriteria == null || !sortCriteria.Any())
            return (IOrderedQueryable<T>)query;

        var firstCriteria = sortCriteria.First();
        var propertyInfo = typeof(T).GetProperty(firstCriteria.Column.CapitalizeFirstLetter());
        if (propertyInfo == null)
        {
            return (IOrderedQueryable<T>)query;
        }
        var orderedQuery = (string.IsNullOrEmpty(firstCriteria.Direction) || firstCriteria.Direction == SortDirectionConst.Ascending)
            ? query.OrderBy(GenerateSelector<T>(firstCriteria.Column.CapitalizeFirstLetter()))
            : query.OrderByDescending(GenerateSelector<T>(firstCriteria.Column.CapitalizeFirstLetter()));

        foreach (var criteria in sortCriteria.Skip(1))
        {
            propertyInfo = typeof(T).GetProperty(firstCriteria.Column.CapitalizeFirstLetter());
            if (propertyInfo == null)
            {
                continue;
            }
            orderedQuery = (string.IsNullOrEmpty(criteria.Direction) || criteria.Direction == SortDirectionConst.Ascending)
                ? orderedQuery.ThenBy(GenerateSelector<T>(criteria.Column.CapitalizeFirstLetter()))
                : orderedQuery.ThenByDescending(GenerateSelector<T>(criteria.Column.CapitalizeFirstLetter()));
        }

        return orderedQuery;
    }

    private static Expression<Func<T, object>> GenerateSelector<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        Expression property = parameter;
        foreach (var member in propertyName.Split('.'))
        {
            property = Expression.Property(property, member);
        }
        var conversion = Expression.Convert(property, typeof(object));
        return Expression.Lambda<Func<T, object>>(conversion, parameter);
    }

    public static string CapitalizeFirstLetter(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        StringBuilder sb = new StringBuilder(input);
        sb[0] = char.ToUpper(sb[0]);
        return sb.ToString();
    }
}
