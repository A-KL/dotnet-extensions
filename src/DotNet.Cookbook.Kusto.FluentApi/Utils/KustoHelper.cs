using System.Collections;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DotNet.Cookbook.Kusto.FluentApi.Utils;

internal class KustoHelper
{
    public static string BuildSetOrAppend(string tableName, IDictionary<string, object?> fields, string? ingestIfNotExists = default)
    {
        var kustoBuilder = new StringBuilder();

        kustoBuilder
            .AppendLine($".set-or-append {tableName}");

        if (ingestIfNotExists != null)
        {
            kustoBuilder
                .AppendLine(
                    $"with (ingestIfNotExists = '[\"{ingestIfNotExists}\"]', tags = '[\"ingest-by:{ingestIfNotExists}\"]')");
        }

        kustoBuilder
            .AppendLine("<| print");

        foreach (var field in fields)
        {
            kustoBuilder
                .AppendLine($"{field.Key} = {WrapValue(field.Value)}");
        }

        return kustoBuilder
            .ToString();
    }

    public static string BuildDelete(string tableName, IEnumerable<string> wheres)
    {
        var kustoBuilder = new StringBuilder()
            .AppendLine($".delete table {tableName} records <|")
            .AppendLine(tableName);

        foreach (var where in wheres)
        {
            kustoBuilder
                .AppendLine(where);
        }

        return kustoBuilder
            .ToString();
    }

    public static IEnumerable<string> WrapValues(IEnumerable<object?> values)
    {
        return values
            .Select(WrapValue);
    }

    public static string WrapToList(IEnumerable<object?> values)
    {
        var arrayAsString = string.Join(',', WrapValues(values));

        return $"({arrayAsString})";
    }

    public static string WrapValue(object? value) => value switch
    {
        null => string.Empty,

        string => $"'{value}'",

        int => $"toint('{value}')",

        long => $"tolong('{value}')",

        DateTime => $"todatetime('{value}')",

        DateTimeOffset => $"todatetime('{value}')",

        bool => $"tobool('{value}')",

        Guid => $"toguid('{value}')",

        JsonArray => $"todynamic('{value}')",

        JsonNode => $"todynamic('{value}')",

        IEnumerable => $"dynamic('{JsonSerializer.Serialize(value)}')",

        _ => throw new Exception($"Unable to wrap {value} of type {value.GetType()}")
    };
}