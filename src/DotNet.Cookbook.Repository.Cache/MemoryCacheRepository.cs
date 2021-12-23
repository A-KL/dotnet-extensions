using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;

using DotNet.Cookbook.Repository.Abstractions;

namespace DotNet.Cookbook.Repository
{
    public class MemoryCacheRepository<T> : MemoryCacheRepository, IRepository<T>
    {
        public MemoryCacheRepository(IRepository repository, IMemoryCache cache, MemoryCacheRepositorySettings<T> settings = default)
            : base(repository, cache, settings)
        { }

        public async Task<IList<T>> GetListAsync(string key, CancellationToken cancellationToken)
        {
            return await GetListAsync<T>(key, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    public class MemoryCacheRepository : IRepository
    {
        private readonly IMemoryCache _cache;
        private readonly IRepository _repository;
        private readonly MemoryCacheRepositorySettings _settings;

        public MemoryCacheRepository(IRepository repository, IMemoryCache cache, MemoryCacheRepositorySettings settings = default)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _settings = settings ?? new MemoryCacheRepositorySettings 
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
        }

        public async Task<IList<T>> GetListAsync<T>(string key, CancellationToken cancellationToken)
        {
            return await _cache
                .GetOrCreateAsync(key, async cacheEntry => 
                {
                    cacheEntry.AbsoluteExpirationRelativeToNow = _settings?.AbsoluteExpirationRelativeToNow;
                    cacheEntry.AbsoluteExpiration = _settings?.AbsoluteExpiration;
                    cacheEntry.SlidingExpiration = _settings?.SlidingExpiration;

                    return await _repository
                        .GetListAsync<T>(key, cancellationToken)
                        .ConfigureAwait(false);
                })
                .ConfigureAwait(false);
        }
    }
}
