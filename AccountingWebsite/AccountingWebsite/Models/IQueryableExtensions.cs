using System.Linq.Expressions;
using System.Linq;
using System;
using System.Reflection;

namespace AccountingWebsite.Models
{
    // Based on this: https://stackoverflow.com/a/21936366
    // These will allow for OrderBy calls that use just the properties name.
    public static class IQueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, PropertyInfo prop)
        {
            return source.OrderBy(ToLambda<T>(prop));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, PropertyInfo prop)
        {
            return source.OrderByDescending(ToLambda<T>(prop));
        }

        private static Expression<Func<T, object>> ToLambda<T>(PropertyInfo prop)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, prop);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
    }
}
