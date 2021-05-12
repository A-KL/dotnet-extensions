using System.Linq;
using System.Collections.Generic;

using Nuke.Common.Tooling;

namespace DotNet.Cookbook.Nuke
{
    public static class OutputExtensions
    {
        public static IEnumerable<string> FirstColumn(this IEnumerable<Output> output, char separator = '\t')
        {
            foreach (var line in output)
            {
                yield return line.Text.Split(separator).First();
            }
        }
    }
}
