using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExpressionBuilder.Attributes;
using ExpressionBuilder.Enums;

namespace ExpressionBuilder
{
    public sealed class SearchExpressionBuilder
    {

        private List<PropertyInfo> classProps;
        public Expression<Func<T, bool>> CreateSearchExpression<T>(object value, SearchOptions mode)
        {
            classProps = FindSearchableProperties<T>();

            var expression = BuildExpressionTree<T>(value, classProps, mode);

            return expression;
        }

        public Expression<Func<T, bool>> CreateSearchExpression<T>(object value, SearchOptions mode, params string[] properties)
        {
            classProps = FindSearchableProperties<T>(properties);

            var expression = BuildExpressionTree<T>(value, classProps, mode);

            return expression;
        }

        #region [Private Methods]

        private List<PropertyInfo> FindSearchableProperties<T>()
        {
            return typeof(T).GetProperties().Where(
                                       prop => Attribute.IsDefined(prop, typeof(Searchable)))
                                       .ToList();
        }

        private List<PropertyInfo> FindSearchableProperties<T>(params string[] properties)
        {
            return typeof(T).GetProperties().Where(
                                       prop => properties.Contains(prop.Name))
                                       .ToList();
        }

        private Expression<Func<T, bool>> BuildExpressionTree<T>(object value, List<PropertyInfo> classProps, SearchOptions searchMode)
        {
            Expression exp = null;
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "x");
            ConstantExpression valueExpression = Expression.Constant(value, value.GetType());

            for (int i = 0; i < classProps.Count; i++)
            {
                Expression eq = null; MemberExpression memberExpression = null;

                if (value.GetType() == classProps[i].PropertyType)
                    memberExpression = Expression.PropertyOrField(parameterExpression, classProps[i].Name);

                if (memberExpression == null) continue;

                if (searchMode == SearchOptions.Equal)
                {
                    if (value.GetType() == typeof(string))
                    {
                        string valueString = value as string ?? "";
                        var toUpperMethodInfo = typeof(string).GetMethod("ToUpper", System.Type.EmptyTypes);
                        var upperExpression = Expression.Call(memberExpression, toUpperMethodInfo);
                        valueExpression = Expression.Constant(valueString.ToUpper(), value.GetType());
                        eq = Expression.Equal(upperExpression, valueExpression);
                    }
                    else
                    {

                        eq = Expression.Equal(memberExpression, valueExpression);
                    }

                }
                else if (searchMode == SearchOptions.Contains)
                {
                    if (value.GetType() != typeof(string)) throw new NotSupportedException("The type of property must be System.String");

                    MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    eq = Expression.Call(memberExpression, method, Expression.Constant(value, typeof(string)));
                }


                if (exp == null)
                    exp = eq;
                else
                    exp = Expression.OrElse(exp, eq);
            }

            return Expression.Lambda<Func<T, bool>>(exp, parameterExpression);
        }
        #endregion
    }
}
