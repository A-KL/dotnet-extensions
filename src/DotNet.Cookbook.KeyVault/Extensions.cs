using System;
using System.Collections.Generic;

using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace DotNet.Cookbook.KeyVault
{
    public static class Extensions
    {
        public static IConfigurationBuilder AddAzureKeyVaultForUri(this IConfigurationBuilder configurationBuilder, Uri uri, IDictionary<string, string> mapping)
        {
            return configurationBuilder.AddAzureKeyVault(
                    uri,
                    new DefaultAzureCredential(),
                    new DelegatedKeyVaultSecretManager(mapping));
        }
    }
}
