using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CosmosDbFramework.Internal.Extensions
{
    public static class PredicateExtensions
    {
        private static ExpressionType[] _conditionalAvailable = new[] { ExpressionType.Equal, ExpressionType.NotEqual, ExpressionType.GreaterThan, ExpressionType.GreaterThanOrEqual, ExpressionType.LessThan, ExpressionType.LessThanOrEqual };
        private static ExpressionType[] _concatenatorsAvailable = new[] { ExpressionType.AndAlso, ExpressionType.OrElse };

        public static string GetCosmosDbQuery<T>(this Expression<Func<T, bool>> predicate, string tableName)
        {
            if (predicate == default)
                return $"SELECT * FROM {tableName}";

            return predicate.Build<T>(tableName);
        }

        private static string CreateWhereClause<T>(this Expression<Func<T, bool>> predicate, string tableName)
        {
            var binaryExpression = (BinaryExpression)predicate.Body;
            return binaryExpression.GetWhereClause(tableName);
        }

        private static string GetWhereClause(this BinaryExpression binaryExpression, string tableName)
        {
            var builder = new StringBuilder();
            binaryExpression.WhereClauseBuilder(builder, tableName);
            return builder.ToString();
        }

        private static void WhereClauseBuilder(this BinaryExpression binaryExpression, StringBuilder builder, string tableName)
        {
            if (_conditionalAvailable.Contains(binaryExpression.NodeType))
            {
                binaryExpression.GetConditional(builder, tableName);
                return;
            }
            else if (_concatenatorsAvailable.Contains(binaryExpression.NodeType))
            {
                if (binaryExpression.NodeType == ExpressionType.AndAlso)                
                    binaryExpression.GetAndConditional(builder, tableName);                
                else
                    binaryExpression.GetOrConditional(builder, tableName);
                
                return;
            }
            else
                throw new NotImplementedException($"Expression Type ({binaryExpression.NodeType}) not implemented.");
        }

        private static void GetAndConditional(this BinaryExpression binaryExpression, StringBuilder builder, string tableName)
        {
           ((BinaryExpression)binaryExpression.Left).WhereClauseBuilder(builder, tableName);
            builder.Append(" AND ");
            ((BinaryExpression)binaryExpression.Right).WhereClauseBuilder(builder, tableName);
        }

        private static void GetOrConditional(this BinaryExpression binaryExpression, StringBuilder builder, string tableName)
        {
            ((BinaryExpression)binaryExpression.Left).WhereClauseBuilder(builder, tableName);
            builder.Append(" OR ");
            ((BinaryExpression)binaryExpression.Right).WhereClauseBuilder(builder, tableName);
        }

        private static void GetConditional(this BinaryExpression binaryExpression, StringBuilder builder, string tableName)
        {
            var memberExpression = (MemberExpression)binaryExpression.Left;
            var constantExpression = (ConstantExpression)binaryExpression.Right;
            var propertyName = memberExpression.GetPropertyName(tableName);
            var operatorFilter = binaryExpression.GetOperator();
            var filter = constantExpression.GetConstant();
            builder.Append($"{propertyName} {operatorFilter} {filter}");
        }

        private static string GetOperator(this BinaryExpression binaryExpression)
        {
            var expressionType = binaryExpression.NodeType;
            switch (expressionType)
            {
                case ExpressionType.Equal:
                    return "=";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.NotEqual:
                    return "!=";
                default:
                    throw new NotImplementedException($"{expressionType.ToString()}");
            }
        }

        private static string GetConstant(this ConstantExpression constantExpression)
        {
            if (constantExpression.Type == typeof(string))
                return $@"""{constantExpression.Value}""";
            else if (constantExpression.Type == typeof(int) || constantExpression.Type == typeof(long))
                return $@"{constantExpression.Value}";
            else
                return constantExpression.Value.ToString();
        }

        private static string GetPropertyName(this MemberExpression memberExpression, string tableName)
        {
            var propertyName = memberExpression.Member.Name;
            var propertyNameArray = propertyName.ToCharArray();
            propertyNameArray[0] = char.ToLower(propertyName[0]);
            propertyName = string.Join(string.Empty, propertyNameArray);
            return $"{tableName}.{propertyName}";
        }

        private static string Build<T>(this Expression<Func<T, bool>> predicate, string tableName)
        {
            var select = CreateSelectCommand(tableName);
            var where = predicate.CreateWhereClause(tableName);
            return $"{select} WHERE {where}";
        }

        private static string CreateSelectCommand(string tableName) => $"SELECT * FROM {tableName}";
    }
}
