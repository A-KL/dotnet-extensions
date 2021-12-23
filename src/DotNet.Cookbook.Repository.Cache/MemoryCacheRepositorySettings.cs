using System;

namespace DotNet.Cookbook.Repository
{
    public class MemoryCacheRepositorySettings<T> 
        : MemoryCacheRepositorySettings
    { }

    public class MemoryCacheRepositorySettings
    {
        public DateTimeOffset? AbsoluteExpiration { get; set; }

        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

        public TimeSpan? SlidingExpiration { get; set; }
    }
}
