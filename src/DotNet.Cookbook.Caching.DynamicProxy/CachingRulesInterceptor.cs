using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;

namespace DotNet.Cookbook.Caching.DynamicProxy
{
    internal class CachingRulesAsyncInterceptor<T> : AsyncInterceptorBase
    {
        private readonly IDictionary<MethodInfo, MethodExecutionContext> _rules;
        private readonly IMemoryCache _cache;

        public CachingRulesAsyncInterceptor(IMemoryCache cache, IDictionary<MethodInfo, MethodExecutionContext> rules)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _rules = rules ?? throw new ArgumentNullException(nameof(rules));
        }

        protected override Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            throw new NotImplementedException();
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            if (_rules.ContainsKey(invocation.Method))
            {
                var key = (string)_rules[invocation.Method]
                    .Invoke(invocation.Arguments);

                return await _cache.GetOrCreateAsync(key, async entry =>
                {
                    //entry.AbsoluteExpirationRelativeToNow = _settings?.AbsoluteExpirationRelativeToNow;
                    //entry.AbsoluteExpiration = _settings?.AbsoluteExpiration;
                    //entry.SlidingExpiration = _settings?.SlidingExpiration;

                    return await proceed(invocation, proceedInfo)
                        .ConfigureAwait(false);

                }).ConfigureAwait(false);
            }

            return await proceed(invocation, proceedInfo)
                .ConfigureAwait(false);
        }
    }
}
