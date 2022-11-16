using DotNet.Cookbook.Kusto.FluentApi.Utils;

namespace DotNet.Cookbook.Kusto.FluentApi.Expressions
{
    public static class DeleteExtensions
    {
        public static IDeleteExpression Delete(this IExtendableBuilder builder, string tableName)
        {
            return new DeleteExpression(tableName);
        }

        //public static IDeleteExpression<T> Delete<T>(this IExtendableBuilder builder, string tableName)
        //{
        //    return new DeleteExpression<T>(tableName);
        //}
    }

    internal class DeleteExpression : IDeleteExpression
    {
        private readonly string _tableName;

        private readonly IList<string> _wheres = new List<string>();

        public DeleteExpression(string tableName)
        {
            _tableName = tableName;
        }

        public IDeleteExpression WhereEquals(string key, object? value)
        {
            _wheres.Add($"| where {key} == {KustoHelper.WrapValue(value)}");

            return this;
        }

        public IDeleteExpression WhereIn(string key, IEnumerable<object?> values)
        {
            _wheres.Add($"| where {key} in {KustoHelper.WrapToList(values)}");

            return this;
        }

        public string Execute()
        {
            return KustoHelper
                .BuildDelete(_tableName, _wheres);
        }
    }

    //internal class DeleteExpression<T> : IDeleteExpression<T>
    //{
    //    private readonly string _tableName;

    //    public DeleteExpression(string tableName)
    //    {
    //        _tableName = tableName;
    //    }

    //    public IDeleteExpression<T> WhereEquals()
    //    {
            
    //        return this;
    //    }

    //    public string Execute()
    //    {
    //        var expression = new DeleteExpression(_tableName);
                
    //        return expression
    //            .Execute()
    //    }
    //}
}
