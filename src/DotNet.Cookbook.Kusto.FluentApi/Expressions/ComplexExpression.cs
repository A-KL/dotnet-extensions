using System.Text;

namespace DotNet.Cookbook.Kusto.FluentApi.Expressions;

public static class ComplexExpressionExtensions
{
    public static IComplexExpression Add(this IComplexExpression exp, Func<IExtendableBuilder, IExpression> builder)
    {
        var expression = builder(ExtendableBuilder.Instance);

        exp.Add(expression);

        return exp;
    }
}

public class ComplexExpression : IComplexExpression
{
    private readonly IList<IExpression> _expressions = new List<IExpression>();

    public static IComplexExpression New()
    {
        return new ComplexExpression();
    }

    public IComplexExpression Add(IExpression expression)
    {
        _expressions.Add(expression);

        return this;
    }

    public string Execute()
    {
        var kustoBuilder = new StringBuilder();

        if (_expressions.Count > 1)
        {
            kustoBuilder
                .AppendLine(".execute database script <|")
                .AppendLine();
        }

        foreach (var expression in _expressions)
        {
            kustoBuilder
                .Append(expression.Execute());
        }

        return kustoBuilder
            .ToString();
    }

    public override string ToString()
    {
        return Execute();
    }
}