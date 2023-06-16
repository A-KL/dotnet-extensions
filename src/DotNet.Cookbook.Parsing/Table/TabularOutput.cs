using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNet.Cookbook.Parsing.Table
{
    /// <summary>
    /// Utils methods to work with tabular data.
    /// </summary>
    public static class TabularOutput
    {
        /// <summary>
        /// Parses the specified input into a collection of <see cref="ReadOnlyRow" />.
        /// </summary>
        public static IReadOnlyCollection<IReadOnlyRow> Parse(string input, bool header = false)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            var lines = input
                .Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            if (!lines.Any())
            {
                return new List<IReadOnlyRow>();
            }

            return lines
                .Skip(header ? 1 : 0)
                .Select(row => row.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(x => new ReadOnlyRow(x))
                .ToList();
        }
    }
}