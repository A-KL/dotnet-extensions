using System.Linq.Expressions;
using DotNet.Cookbook.Kusto.FluentApi.Utils;

namespace DotNet.Cookbook.Kusto.FluentApi.Expressions;

internal class SetOrAppendExpression : ISetOrAppendExpression
{
    private readonly string _tableName;
    private string? _ingestIfNotExists;

    private readonly IDictionary<string, object?> _fields = new Dictionary<string, object?>();

    public SetOrAppendExpression(string tableName)
    {
        _tableName = tableName;
    }

    public ISetOrAppendExpression With(string ingestIfNotExists)
    {
        if (string.IsNullOrEmpty(ingestIfNotExists))
        {
            throw new ArgumentNullException(nameof(ingestIfNotExists));
        }

        _ingestIfNotExists = ingestIfNotExists;

        return this;
    }

    public ISetOrAppendExpression Include(string name, object? value)
    {
        _fields.Add(name, value);

        return this;
    }

    public string Execute()
    {
        return KustoHelper
            .BuildSetOrAppend(_tableName, _fields, _ingestIfNotExists);
    }
}

internal class SetOrAppendExpression<T> : ISetOrAppendExpression<T>
{
    private readonly string _tableName;
    private Func<T, string>? _ingestIfNotExists;

    private readonly IDictionary<string, Func<T, object?>> _fields = new Dictionary<string, Func<T, object?>>();

    public SetOrAppendExpression(string tableName)
    {
        _tableName = tableName;
    }

    public ISetOrAppendExpression<T> With(Expression<Func<T, string>> ingestIfNotExists)
    {
        _ingestIfNotExists = ingestIfNotExists.Compile();

        return this;
    }

    public ISetOrAppendExpression<T> Include(Expression<Func<T, object?>> valueFuncExp)
    {
        var name = ExpressionHelper.GetPropertyName(valueFuncExp);
        var valueFunc = valueFuncExp.Compile();

        _fields.Add(name, valueFunc);

        return this;
    }

    public ISetOrAppendExpression<T> Include<TOutput>(Expression<Func<T, TOutput?>> valueFuncExp, Func<TOutput?, object?> formatFunc)
    {
        var name = ExpressionHelper.GetPropertyName(valueFuncExp);
        var valueFunc = valueFuncExp.Compile();

        _fields.Add(name, x => formatFunc(valueFunc(x)));

        return this;
    }

    public IExpression Execute(T instance)
    {
        var command = new SetOrAppendExpression(_tableName);

        if (_ingestIfNotExists != null)
        {
            command
                .With(_ingestIfNotExists(instance));
        }

        foreach (var field in _fields)
        {
            command
                .Include(field.Key, field.Value(instance));
        }

        return command;
    }
}