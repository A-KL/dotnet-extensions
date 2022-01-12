using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotNet.Cookbook.Caching.DynamicProxy.Tests
{
    public class TestAddCaching
    {
        [Fact]
        public void FooClient_Second_Call_Gets_Cached()
        {
            // Arrange
            var provider = new ServiceCollection()
                .AddMemoryCache()

                .AddSingleton<IProxyGenerator, ProxyGenerator>()

                .AddSingleton<IFooClient, FooClient>()

                .AddCaching<IFooClient>(builder => builder
                    .ForMethod<string, CancellationToken, Task<string>>(
                        x => x.GetAsync,
                        (arg1, arg2) => arg1 + "123"))

                .BuildServiceProvider();

            var client = provider.GetRequiredService<IFooClient>();

            // Assert
            Assert.NotNull(client);

            Assert.True(
                Utils.DurationOf(() =>
                {
                    Assert.Equal("test_arg1", client.GetAsync("test_arg1", CancellationToken.None).Result); 
                }) 
                > TimeSpan.FromSeconds(4));

            Assert.True(
                Utils.DurationOf(() =>
                {
                    Assert.Equal("test_arg1", client.GetAsync("test_arg1", CancellationToken.None).Result);
                })
                <= TimeSpan.FromSeconds(1));
        }
    }
}