using Microsoft.Extensions.DependencyInjection;

using DotNet.Cookbook.Caching.Abstractions;

namespace DotNet.Cookbook.Caching
{
    public static class Extensions
    {
        /// <summary>
        /// Registers singleton cache instance for a specific type.
        /// </summary>
        /// <typeparam name="TValue">Caching type</typeparam>
        /// <param name="collection">Instance of IServiceCollection</param>
        /// <param name="name">Caching key prefix</param>
        /// <returns>Instance of IServiceCollection</returns>
        public static IServiceCollection AddCacheFor<TValue>(this IServiceCollection collection, string name)
        {
            return collection
                .AddSingleton<ICache<TValue>>(x=> 
                    new GenericCacheDecorator<TValue>(
                        new SimpleMemoryCache(name)));
        }

        /// <summary>
        /// Registers a threadsafe singleton cache instance for a specific type.
        /// </summary>
        /// <typeparam name="TValue">Caching type</typeparam>
        /// <param name="collection">Instance of IServiceCollection</param>
        /// <param name="name">Caching key prefix</param>
        /// <returns>Instance of IServiceCollection</returns>
        public static IServiceCollection AddThreadSafeCacheFor<TValue>(this IServiceCollection collection, string name)
        {
            return collection
                .AddSingleton<ICache<TValue>>(x => 
                    new ThreadSafeCacheDecorator<TValue>(
                        new GenericCacheDecorator<TValue>(
                            new SimpleMemoryCache(name))));
        }
    }
}
