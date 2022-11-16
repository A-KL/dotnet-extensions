namespace DotNet.Cookbook.Kusto.FluentApi.Tests;

public class SetOrAppendTests
{
    private static readonly string cr = Environment.NewLine;

    [Fact]
    public void TestIntFieldHasWrappedValue()
    {
        var command = new ComplexExpression()
            .Add(x => x
            .SetOrAppend("TestTable")
            .Include("IntField", 123))
            .Execute();

        Assert.Equal($".set-or-append TestTable{cr}<| print{cr}IntField = toint('123'){cr}", command);
    }

    [Fact]
    public void TestDateTimeFieldHasWrappedValue()
    {
        var table = "TestTable";
        var name = "DateTimeField";
        var value = DateTime.Now;

        var command = new SetOrAppendExpression(table)
            .Include(name, value)
            .Execute();

        Assert.Equal($".set-or-append {table}{cr}<| print{cr}{name} = todatetime('{value}'){cr}", command);
    }

    [Fact]
    public void TestDateTimeOffsetFieldHasWrappedValue()
    {
        var table = "TestTable";
        var name = "DateTimeField";
        var value = DateTimeOffset.Now;

        var command = new SetOrAppendExpression(table)
            .Include(name, value)
            .Execute();

        Assert.Equal($".set-or-append {table}{cr}<| print{cr}{name} = todatetime('{value}'){cr}", command);
    }

    [Fact]
    public void TestStringFieldHasWrappedValue()
    {
        var table = "TestTable";
        var name = "StringField";
        var value = "test sting value";

        var command = new SetOrAppendExpression(table)
            .Include(name, value)
            .Execute();

        Assert.Equal($".set-or-append {table}{cr}<| print{cr}{name} = '{value}'{cr}", command);
    }

    [Fact]
    public void TestStringFieldExpressionHasWrappedValue()
    {
        var table = "TestTable";

        var model = new TestModel();

        var command = new SetOrAppendExpression<TestModel>(table)
            .Include(x => x.StringProperty)
            .Execute(model)
            .Execute();

        Assert.Equal($".set-or-append {table}{cr}<| print{cr}{TestModel.StringPropertyName} = '{TestModel.StringPropertyValue}'{cr}", command);
    }

    [Fact]
    public void TestArrayFieldExpressionHasWrappedValue()
    {
        var table = "TestTable";

        var model = new TestModel();

        var command = new SetOrAppendExpression<TestModel>(table)
            .Include(x => x.StringArrayProperty)
            .Execute(model)
            .Execute();

        var json = JsonSerializer.Serialize(model.StringArrayProperty);

        Assert.Equal($".set-or-append {table}{cr}<| print{cr}{TestModel.StringArrayPropertyName} = dynamic('{json}'){cr}", command);
    }

    [Fact]
    public void TestGuidToStringFieldExpressionHasWrappedValue()
    {
        var table = "TestTable";

        var model = new TestModel();

        var command = new SetOrAppendExpression<TestModel>(table)
            .Include(x => x.GuidProperty, x => x.ToString())
            .Execute(model)
            .Execute();

        Assert.Equal($".set-or-append {table}{cr}<| print{cr}{TestModel.GuidPropertyName} = '{model.GuidProperty}'{cr}", command);
    }

    [Fact]
    public void TestTwoTables()
    {
        var table1 = "TestTable1";
        var table2 = "TestTable2";

        var name1 = "StringField";
        var value1 = "test sting value";

        var name2 = "DateTimeField";
        var value2 = DateTimeOffset.Now;

        var command = new ComplexExpression()
            .Add(x => x
                .SetOrAppend(table1)
                .Include(name1, value1))
            .Add(x => x
                .SetOrAppend(table2)
                .Include(name2, value2))
            .Execute();

        Assert.Equal($".execute database script <|{cr}{cr}" +
                     $".set-or-append {table1}{cr}<| print{cr}{name1} = '{value1}'{cr}" +
                     $".set-or-append {table2}{cr}<| print{cr}{name2} = todatetime('{value2}'){cr}", command);
    }
}