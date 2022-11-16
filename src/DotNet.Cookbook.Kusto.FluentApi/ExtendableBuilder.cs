namespace DotNet.Cookbook.Kusto.FluentApi;

internal sealed class ExtendableBuilder : IExtendableBuilder
{
    public static readonly IExtendableBuilder Instance = new ExtendableBuilder();
}