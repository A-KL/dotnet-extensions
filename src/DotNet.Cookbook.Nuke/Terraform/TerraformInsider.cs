using System.Collections.Generic;
using System.IO;

namespace DotNet.Cookbook.Nuke.Terraform
{
    public class TerraformInsider
    {
        public static string GetProvider()
        {
            foreach (string fileName in TerraformFiles("main.tf"))
            {
                foreach (var line in File.ReadLines(fileName))
                {
                    if (line.StartsWith("provider"))
                    {
                        return line
                            .Substring("provider".Length)
                            .Replace("\"", string.Empty)
                            .Replace("{", string.Empty)
                            .Trim();
                    }
                }
            }

            return null; // TODO: find a way to get rid of this
        }

        private static IEnumerable<string> TerraformFiles(string defaultFile = default)
        {
            if (defaultFile != null && File.Exists(defaultFile))
            {
                yield return defaultFile;
                yield break;
            }

            foreach (string fileName in Directory.GetFiles("*.tf"))
            {
                yield return fileName;
            }
        }
    }
}
