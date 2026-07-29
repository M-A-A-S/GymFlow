using GymFlow.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymFlow.Infrastructure.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PagedResult<T>> ToPagedListAsync<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            var totalCount = await query.CountAsync();

            var items =
                await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

        }
    

        public static IQueryable<T> OrderByProperty<T>(
            this IQueryable<T> source,
            string propertyName,
            bool descending)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return source;
            }

            var property = typeof(T).GetProperty(propertyName);

            if (property == null)
            {
                return source;
            }

            var parameter = Expression.Parameter(typeof(T), "x");

            var propertyAccess =
                Expression.MakeMemberAccess(parameter, property);

            var orderByExp =
                Expression.Lambda(propertyAccess, parameter);

            string method =
                descending
                    ? "OrderByDescending"
                    : "OrderBy";

            var result =
                Expression.Call(
                    typeof(Queryable),
                    method,
                    new[]
                    {
                        typeof(T),
                        property.PropertyType
                    },
                    source.Expression,
                    Expression.Quote(orderByExp));

                return source.Provider.CreateQuery<T>(result);

        }

    }
}
