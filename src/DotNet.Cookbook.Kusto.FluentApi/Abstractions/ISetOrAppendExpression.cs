using System.Linq.Expressions;

namespace DotNet.Cookbook.Kusto.FluentApi.Abstractions;

public interface ISetOrAppendExpression : IExpression
{
    ISetOrAppendExpression With(string ingestIfNotExists);

    ISetOrAppendExpression Include(string name, object? value);
}

public interface ISetOrAppendExpression<T> : IExpression<T, IExpression>
{
    ISetOrAppendExpression<T> With(Expression<Func<T, string>> ingestIfNotExists);

    ISetOrAppendExpression<T> Include(Expression<Func<T, object?>> valueFuncExp);

    ISetOrAppendExpression<T> Include<TOutput>(Expression<Func<T, TOutput?>> valueFuncExp, Func<TOutput?, object?> formatFunc);
}