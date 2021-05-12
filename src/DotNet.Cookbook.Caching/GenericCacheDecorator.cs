using System;

using DotNet.Cookbook.Caching.Abstractions;

namespace DotNet.Cookbook.Caching
{
    /// <summary>
    /// Generic decorator for a simple ICache<object>. Used to perform conversion from object to TValue.
    /// </summary>
    /// <typeparam name="TValue">Cache type.</typeparam>
    public class GenericCacheDecorator<TValue> : ICache<TValue>
    {
        private readonly ICache<object> _cache;

        public GenericCacheDecorator(ICache<object> cache) 
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public TValue GetValue(string key)
        {
            return (TValue)_cache.GetValue(key);
        }

        void ICache<TValue>.SetValue(string key, TValue value, DateTimeOffset expiration)
        {
            _cache.SetValue(key, value, expiration);
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}