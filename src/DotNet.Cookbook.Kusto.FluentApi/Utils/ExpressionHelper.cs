using System.Linq.Expressions;
using System.Reflection;

namespace DotNet.Cookbook.Kusto.FluentApi.Utils;

internal static class ExpressionHelper
{
    public static string GetPropertyName<T, TResult>(Expression<Func<T, TResult?>> property)
    {
        var lambda = (LambdaExpression)property;
        MemberExpression memberExpression;

        if (lambda.Body is UnaryExpression unaryExpression)
        {
            memberExpression = (MemberExpression)unaryExpression.Operand;
        }
        else
        {
            memberExpression = (MemberExpression)lambda.Body;
        }

        return ((PropertyInfo)memberExpression.Member).Name;
    }

    public static string GetPropertyName<T>(Expression<Func<T, object?>> property)
    {
        var lambda = (LambdaExpression)property;
        MemberExpression memberExpression;

        if (lambda.Body is UnaryExpression unaryExpression)
        {
            memberExpression = (MemberExpression)unaryExpression.Operand;
        }
        else
        {
            memberExpression = (MemberExpression)lambda.Body;
        }

        return ((PropertyInfo)memberExpression.Member).Name;
    }

    public static string GetPropertyName(Expression<Func<object?>> property)
    {
        var lambda = (LambdaExpression)property;
        MemberExpression memberExpression;

        if (lambda.Body is UnaryExpression unaryExpression)
        {
            memberExpression = (MemberExpression)unaryExpression.Operand;
        }
        else
        {
            memberExpression = (MemberExpression)lambda.Body;
        }

        return ((PropertyInfo)memberExpression.Member).Name;
    }
}