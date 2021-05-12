using DotNet.Cookbook.Caching.Abstractions;
using System;

namespace DotNet.Cookbook.Caching
{
    /// <summary>
    /// Threadsafe ICache<TValue> decorator.
    /// </summary>
    /// <typeparam name="TValue">Cache type.</typeparam>
    public class ThreadSafeCacheDecorator<TValue> : ICache<TValue>
    {
        private readonly object _lock = new object();
        private readonly ICache<TValue> _cache;

        public ThreadSafeCacheDecorator(ICache<TValue> cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public TValue GetValue(string key)
        {
            return _cache.GetValue(key);
        }

        void ICache<TValue>.SetValue(string key, TValue value, DateTimeOffset expiration)
        {
            lock (_lock)
            {
                _cache.SetValue(key, value, expiration);
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                _cache.Dispose();
            }
        }
    }
}