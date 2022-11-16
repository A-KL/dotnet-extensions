namespace DotNet.Cookbook.Kusto.FluentApi.Tests;

internal sealed class TestModel
{
    public string StringProperty => StringPropertyValue;

    public static string StringPropertyName = nameof(StringProperty);

    public static string StringPropertyValue = "StringValue";


    public string[] StringArrayProperty => StringArrayPropertyValue;

    public static string StringArrayPropertyName = nameof(StringArrayProperty);

    public static string[] StringArrayPropertyValue = { "value 1", "value 2", "value 3" };


    public Guid GuidProperty => GuidPropertyValue;

    public static string GuidPropertyName = nameof(GuidProperty);

    public static Guid GuidPropertyValue = Guid.NewGuid();
}