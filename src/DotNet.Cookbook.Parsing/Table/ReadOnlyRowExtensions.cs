using System;
using System.Collections.Generic;

namespace DotNet.Cookbook.Parsing.Table
{
    /// <summary>
    /// Represents a collection of extension methods for <see cref="ReadOnlyRow" />.
    /// </summary>
    public static class ReadOnlyRowExtensions
    {
        /// <summary>
        /// Gets a value of the specified column and performs type conversion.
        /// </summary>
        public static IEnumerable<T> GetColumn<T>(this IEnumerable<ReadOnlyRow> rows, int columnIndex)
        {
            if (rows == null)
            {
                throw new ArgumentNullException(nameof(rows));
            }

            if (columnIndex < 0)
            {
                throw new ArgumentException("Parameter should have a positive value", nameof(columnIndex));
            }

            foreach (var tableRow in rows)
            {
                yield return tableRow.Column<T>(columnIndex);
            }
        }
    }
}