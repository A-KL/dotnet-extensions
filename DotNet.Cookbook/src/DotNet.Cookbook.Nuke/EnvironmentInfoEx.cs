using System.Linq;
using System.Collections.Generic;

using Nuke.Common;

using static Nuke.Common.EnvironmentInfo;

namespace DotNet.Cookbook.Nuke
{
    static class EnvironmentInfoEx
    {
        /// <summary>
        /// Filters env variables by prefix.
        /// </summary>
        /// <param name="prefix">Env variable prefix</param>
        /// <returns>Formatted string: "-var Key1=Value1 -var Key2=Value2 ..."</returns>
        public static string GetTerraformTemplateParameters(string prefix)
        {
            var values = GetEnvironmentVariables(prefix)
                        .Select(x => $"-var {x.Key.ToLower()}={x.Value}");

            return string
                .Join(" ", values);
        }

        /// <summary>
        /// Gets variables by prefix as dictionary.
        /// </summary>
        /// <param name="prefix">Env variable prefix</param>
        /// <returns>A dictionary of env var name : value</returns>
        public static IDictionary<string, string> GetEnvironmentVariables(string prefix)
        {
            var result = new Dictionary<string, string>();

            prefix = prefix.ToUpper().Replace('.', '_');

            foreach (var variable in Variables)
            {
                var key = variable.Key;

                if (key.StartsWith(prefix))
                {
                    var variableKey = key.Substring(prefix.Length);
                    var variableValue = variable.Value;

                    Logger.Normal($"Variable(s) prefixed by '{prefix}' was(were) found \'{variableKey}={variableValue}\'");

                    result.Add(variableKey, variableValue);
                }
            }

            return result;
        }
    }

}
