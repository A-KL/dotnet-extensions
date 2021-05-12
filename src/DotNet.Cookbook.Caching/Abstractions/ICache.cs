using System;

namespace DotNet.Cookbook.Caching.Abstractions
{
    public interface ICache<TValue> : IDisposable
    {
        TValue GetValue(string key);

        void SetValue(string key, TValue value, DateTimeOffset expiration);
    }
}