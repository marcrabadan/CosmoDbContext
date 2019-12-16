using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CosmosDbFramework.Internal.Extensions
{
    public static class PartitionKeyPathExtensions
    {
        public static string ToPartitionKeyPath<TDocument>(this Expression<Func<TDocument, object>> expression)
        {
            var partitionKeyProperty = GetPartitionKeyPropertyName(expression);
            return $"/{partitionKeyProperty.ToLowerInvariant()}";
        }

        public static object GetPartitionKeyValue<TDocument>(this TDocument document, Expression<Func<TDocument, object>> expression)
        {
            var propertyName = GetPartitionKeyPropertyName(expression);
            var property = document.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            return property.GetValue(document);
        }

        private static string GetPartitionKeyPropertyName<TDocument>(this Expression<Func<TDocument, object>> expression)
        {
            if (expression.NodeType == ExpressionType.Lambda)
                if (expression.Body.NodeType == ExpressionType.MemberAccess)
                {
                    var memberExpression = expression.Body as MemberExpression;
                    return memberExpression.Member.Name;
                }

            throw new ArgumentNullException(nameof(expression));
        }
    }
}
