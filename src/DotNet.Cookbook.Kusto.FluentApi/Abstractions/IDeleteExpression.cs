namespace DotNet.Cookbook.Kusto.FluentApi.Abstractions;

public interface IDeleteExpression : IExpression
{
    IDeleteExpression WhereEquals(string key, object? value);

    IDeleteExpression WhereIn(string key, IEnumerable<object?> values);
}

//public interface IDeleteExpression<T> : IExpression<T, IExpression>
//{
//    ISetOrAppendExpression WhereEquals(Expression<Func<T, object?>> valueFuncExp);
//}