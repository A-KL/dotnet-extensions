namespace DotNet.Cookbook.Parsing.Table;

/// <summary>
/// Represents a read-only row of tabular data.
/// </summary>
public interface IReadOnlyRow
{
    /// <summary>
    /// Gets the values of the row.
    /// </summary>
    string this[int index] { get; }

    /// <summary>
    /// Gets the value of the column at the specified index.
    /// </summary>
    T Column<T>(int columnIndex);
}