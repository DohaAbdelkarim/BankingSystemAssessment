using System;
using System.Linq;
using System.Linq.Expressions;

namespace BankingSystemAssessment.Core.Extensions.EnumerableExtensions
{
    public static class SortExtension
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> source, Expression<Func<T, object>> predicate, string sortDirection)
        {
            if (sortDirection?.ToLower() == "asc")
            {
                return source.OrderBy(predicate);
            }
            else //The default sortDirection is desc
            {
                return source.OrderByDescending(predicate);
            }
        }
    }
}