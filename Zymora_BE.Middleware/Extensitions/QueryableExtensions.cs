using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zymora_BE.Middleware.Extensitions
{
    public static class QueryableExtensions
    {
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, PaginationParams pagination)
    {
      return query
          .Skip((pagination.PageNumber - 1) * pagination.PageSize)
          .Take(pagination.PageSize);
    }

    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string sortBy, string sortOrder)
    {
      if (string.IsNullOrEmpty(sortBy))
        return query;

      var parameter = Expression.Parameter(typeof(T), "x");
      var property = Expression.Property(parameter, sortBy);
      var lambda = Expression.Lambda(property, parameter);

      var methodName = sortOrder?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

      var resultExpression = Expression.Call(
          typeof(Queryable),
          methodName,
          new Type[] { typeof(T), property.Type },
          query.Expression,
          Expression.Quote(lambda));

      return query.Provider.CreateQuery<T>(resultExpression);
    }

    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, string propertyName, string value)
    {
      if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(value))
        return query;

      var parameter = Expression.Parameter(typeof(T), "x");
      var property = Expression.Property(parameter, propertyName);
      var constant = Expression.Constant(value);

      var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
      var containsExpression = Expression.Call(property, containsMethod, constant);

      var lambda = Expression.Lambda<Func<T, bool>>(containsExpression, parameter);

      return query.Where(lambda);
    }
  }
}
