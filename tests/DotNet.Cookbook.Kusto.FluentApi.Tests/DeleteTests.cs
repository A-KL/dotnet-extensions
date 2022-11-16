namespace DotNet.Cookbook.Kusto.FluentApi.Tests;

public class DeleteTests
{
    private static readonly string cr = Environment.NewLine;

    [Fact]
    public void DeleteWhereEqualByGuidIdGeneratesProperCommand()
    {
        var table = "TestTable";
        var model = new TestModel();

        var command = new DeleteExpression(table)
            .WhereEquals("Id", model.GuidProperty)
            .Execute();

        Assert.Equal($".delete table {table} records <|{cr}{table}{cr}| where Id == toguid('{model.GuidProperty}'){cr}", command);
    }

    [Fact]
    public void DeleteWhereInByStringIdGeneratesProperCommand()
    {
        var table = "TestTable";
        var model = new TestModel();

        var command = new DeleteExpression(table)
            .WhereIn("Id", model.StringArrayProperty)
            .Execute();

        Assert.Equal(
            $".delete table {table} records <|{cr}{table}{cr}| where Id in ('{model.StringArrayProperty[0]}','{model.StringArrayProperty[1]}','{model.StringArrayProperty[2]}'){cr}",
            command);
    }
}
