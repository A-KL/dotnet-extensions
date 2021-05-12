using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace DotNet.Cookbook.DocumentDB
{
    public interface IDocumentClient<T>
    {
        Task<IEnumerable<T>> WhereAllAsync(Expression<Func<T, bool>> predicate, CancellationToken token);
    }
}
