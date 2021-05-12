using System;
using System.Runtime.Caching;

using DotNet.Cookbook.Caching.Abstractions;

namespace DotNet.Cookbook.Caching
{
    /// <summary>
    /// Basic implemntation of ICache<T> using MemoryCache.
    /// </summary>
    public class SimpleMemoryCache : ICache<object>
    {
        private readonly MemoryCache _cache;

        public SimpleMemoryCache(MemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public SimpleMemoryCache(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _cache = new MemoryCache(name);
        }

        public void SetValue(string key, object value, DateTimeOffset expiration)
        {
            _cache.Set(key, value, expiration);
        }

        public object GetValue(string key)
        {
            return _cache.Get(key);
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}