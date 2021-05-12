using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

using Microsoft.Azure.Documents;

namespace DotNet.Cookbook.DocumentDB
{
    public class DocumentClient<T> : IDocumentClient<T>
    {
        private readonly IDocumentClient _client;
        private readonly DocumentCollectionSettings _settings;

        public DocumentClient(IDocumentClientFactory factory, DocumentCollectionSettings settings)
            : this(factory.Create(), settings)
        {}

        public DocumentClient(IDocumentClient client, DocumentCollectionSettings settings)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<IEnumerable<T>> WhereAllAsync(Expression<Func<T, bool>> predicate, CancellationToken token)
        {
            return await _client
                .WhereAllAsync(
                    _settings.Name,
                    _settings.Collection,
                    predicate,
                    _settings.MaxToReturn,
                    token)
                .ConfigureAwait(false);
        }
    }
}
