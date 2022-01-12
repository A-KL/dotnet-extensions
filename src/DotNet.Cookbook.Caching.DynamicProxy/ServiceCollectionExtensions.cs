using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet.Cookbook.Caching.DynamicProxy
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching<T>(this IServiceCollection services, Action<CachingRulesBuilder<T>> configureRules)
            where T : class
        {
            return services
                .Decorate<T>((inner, provider) => {

                    var generator = provider.GetRequiredService<IProxyGenerator>();
                    var cache = provider.GetRequiredService<IMemoryCache>();

                    var builder = new CachingRulesBuilder<T>();

                    configureRules(builder);

                    var interceptor = new CachingRulesAsyncInterceptor<T>(cache, builder.Rules);

                    return generator
                        .CreateInterfaceProxyWithTargetInterface(inner, interceptor);
                });
        }
    }
}
