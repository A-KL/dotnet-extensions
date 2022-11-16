namespace DotNet.Cookbook.Kusto.FluentApi.Abstractions;

public interface IComplexExpression : IExpression
{
    IComplexExpression Add(IExpression expression);
}