using System;
using System.Collections.Generic;

using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;

namespace DotNet.Cookbook.KeyVault
{
    public class DelegatedKeyVaultSecretManager : KeyVaultSecretManager
    {
        private readonly IDictionary<string, string> _mapping;

        public DelegatedKeyVaultSecretManager(IDictionary<string, string> map)
        {
            _mapping = map ?? throw new ArgumentNullException(nameof(map));
        }

        public override string GetKey(KeyVaultSecret secret)
        {
            if (_mapping.ContainsKey(secret.Name))
            {
                return _mapping[secret.Name];
            }

            return base.GetKey(secret);
        }
    }
}
