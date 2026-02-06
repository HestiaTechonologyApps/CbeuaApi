using Microsoft.EntityFrameworkCore;
using Cbeua.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cbeua.Core.Helpers
{
    /// <summary>
    /// Generic pagination helper that can be used with any IQueryable
    /// </summary>
    public static class PaginationHelper
    {
        /// <summary>
        /// Apply pagination to any IQueryable and return PagedResult
        /// </summary>
        public static async Task<PagedResult<T>> ToPaginatedListAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize) where T : class
        {
            // Get total count before pagination
            var totalRecords = await query.CountAsync();

            // Apply pagination
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Data = data,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// Apply search filter to IQueryable based on multiple string properties
        /// </summary>
        public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            string searchTerm,
            params Expression<Func<T, string>>[] searchProperties) where T : class
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || searchProperties == null || searchProperties.Length == 0)
                return query;

            var searchTermLower = searchTerm.ToLower();
            Expression<Func<T, bool>>? combinedExpression = null;

            foreach (var property in searchProperties)
            {
                var parameter = property.Parameters[0];
                var propertyAccess = property.Body;

                // Create: property != null && property.ToLower().Contains(searchTermLower)
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                // Null check
                var notNullCheck = Expression.NotEqual(propertyAccess, Expression.Constant(null, typeof(string)));

                var toLowerCall = Expression.Call(propertyAccess, toLowerMethod);
                var containsCall = Expression.Call(
                    toLowerCall,
                    containsMethod,
                    Expression.Constant(searchTermLower)
                );

                // Combine null check with contains
                var condition = Expression.AndAlso(notNullCheck, containsCall);
                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);

                combinedExpression = combinedExpression == null
                    ? lambda
                    : Expression.Lambda<Func<T, bool>>(
                        Expression.OrElse(combinedExpression.Body, Expression.Invoke(lambda, parameter)),
                        parameter);
            }

            return combinedExpression != null ? query.Where(combinedExpression) : query;
        }

        /// <summary>
        /// Apply sorting to IQueryable
        /// </summary>
        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> query,
            string sortBy,
            bool sortDescending,
            Dictionary<string, Expression<Func<T, object>>> sortMappings) where T : class
        {
            if (string.IsNullOrWhiteSpace(sortBy) || sortMappings == null)
                return query;

            var sortByLower = sortBy.ToLower();

            if (sortMappings.TryGetValue(sortByLower, out var sortExpression))
            {
                return sortDescending
                    ? query.OrderByDescending(sortExpression)
                    : query.OrderBy(sortExpression);
            }

            return query;
        }
    }
}