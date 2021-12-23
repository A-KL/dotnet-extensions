using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Cookbook.Repository.Abstractions
{
    public interface IRepository<T>
    {
        Task<IList<T>> GetListAsync(string key, CancellationToken cancellationToken);
    }

    public interface IRepository
    {
        Task<IList<T>> GetListAsync<T>(string key, CancellationToken cancellationToken);
    }
}
