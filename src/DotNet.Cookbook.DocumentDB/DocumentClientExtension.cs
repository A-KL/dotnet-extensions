using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace DotNet.Cookbook.DocumentDB
{
    public static class DocumentClientExtension
    {
        public static async Task<IEnumerable<T>> WhereAllAsync<T>(this IDocumentClient documentClient, string dbName, string dbCollection, Expression<Func<T, bool>> predicate, int maxItems, CancellationToken token)
        {
            var url = UriFactory.CreateDocumentCollectionUri(
                dbName,
                dbCollection);

            var results = new List<T>();

            var options = new FeedOptions
            {
                MaxItemCount = 1000,
                EnableCrossPartitionQuery = true
            };

            do
            {
                var response = await documentClient
                    .CreateDocumentQuery<T>(url, options)
                    .Where(predicate)
                    .AsDocumentQuery()
                    .ExecuteNextAsync<T>(token)
                    .ConfigureAwait(false);

                options.RequestContinuation = response.ResponseContinuation;

                results.AddRange(response);
            }
            while (options.RequestContinuation != null && results.Count < maxItems);

            return results;
        }
    }
}
