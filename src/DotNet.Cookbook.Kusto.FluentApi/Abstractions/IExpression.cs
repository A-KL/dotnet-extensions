namespace DotNet.Cookbook.Kusto.FluentApi.Abstractions;
public interface IExpression
{
    string Execute();
}

public interface IExpression<in T, out TResult>
{
    TResult Execute(T instance);
}