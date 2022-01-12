using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Cookbook.Caching.DynamicProxy.Tests
{
    internal class FooClient : IFooClient
    {
        public async Task<string> GetAsync(string pram, CancellationToken token)
        {
            await Task.Delay(5000);

            return pram;
        }

        public Task<string> GetAsync2(string pram, string paramm2, CancellationToken token)
        {
            return Task.FromResult(pram + paramm2);
        }
    }
}
