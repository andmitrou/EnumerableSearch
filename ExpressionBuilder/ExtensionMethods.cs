using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionBuilder;
using ExpressionBuilder.Enums;

namespace ExpressionBuilder
{
    public static class ExtensionMethods
    {
        public static IEnumerable<T> Search<T>(this IEnumerable<T> collection, object searchTerm, SearchOptions mode)
        {
            var builder = new ExpressionBuilder.SearchExpressionBuilder();

            var searchExpression = builder.CreateSearchExpression<T>(searchTerm, mode);

            return collection.Where(searchExpression.Compile());
        }

        public static IEnumerable<T> Search<T>(this IEnumerable<T> collection, object searchTerm, SearchOptions mode, params string[] searchProperties)
        {
            var builder = new ExpressionBuilder.SearchExpressionBuilder();

            var searchExpression = builder.CreateSearchExpression<T>(searchTerm, mode, searchProperties);

            return collection.Where(searchExpression.Compile());
        }
    }
}
