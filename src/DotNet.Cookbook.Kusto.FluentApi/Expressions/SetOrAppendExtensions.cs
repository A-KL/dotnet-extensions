namespace DotNet.Cookbook.Kusto.FluentApi.Expressions;

public static class SetOrAppendExtensions
{
    public static ISetOrAppendExpression SetOrAppend(this IExtendableBuilder builder, string tableName)
    {
        return new SetOrAppendExpression(tableName);
    }

    public static ISetOrAppendExpression<T> SetOrAppend<T>(this IExtendableBuilder builder, string tableName)
    {
        return new SetOrAppendExpression<T>(tableName);
    }
}