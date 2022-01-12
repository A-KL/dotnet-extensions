using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Cookbook.Caching.DynamicProxy.Tests
{
    public interface IFooClient
    {
        Task<string> GetAsync(string pram, CancellationToken token);

        Task<string> GetAsync2(string pram, string paramm2, CancellationToken token);
    }
}
