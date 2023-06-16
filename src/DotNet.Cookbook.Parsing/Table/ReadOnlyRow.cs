using System;
using System.Collections.Generic;

namespace DotNet.Cookbook.Parsing.Table
{
    /// <inheritdoc />
    public class ReadOnlyRow : IReadOnlyRow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRow" /> class.
        /// </summary>
        public ReadOnlyRow(IReadOnlyList<string> values)
        {
            Values = values;
        }

        /// <summary>
        /// Represents a list of values in the row.
        /// </summary>
        public IReadOnlyList<string> Values { get; }

        /// <inheritdoc />
        public string this[int index] => Values[index];

        /// <inheritdoc />
        public T Column<T>(int columnIndex)
        {
            return (T)Convert.ChangeType(Values[columnIndex], typeof(T), null);
        }
    }
}