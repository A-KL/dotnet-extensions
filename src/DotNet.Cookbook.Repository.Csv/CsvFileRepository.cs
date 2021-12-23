using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using CsvHelper;
using CsvHelper.Configuration;
using DotNet.Cookbook.Repository.Abstractions;

namespace DotNet.Cookbook.Repository.Csv
{
    public class CsvFileRepository : IRepository
    {
        private readonly HttpClient _client;
        private readonly CsvConfiguration _configuration;

        public CsvFileRepository(HttpClient client, CsvConfiguration? configuration = default)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));

            _configuration = configuration ?? new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = true };
        }

        public async Task<IList<T>> GetListAsync<T>(string key, CancellationToken cancellationToken)
        {
            var response = await _client
                .GetAsync(key, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content
                .ReadAsStreamAsync()
                .ConfigureAwait(false);

            using (content)
            using (var reader = new StreamReader(content))
            using (var csv = new CsvReader(reader, _configuration))
            {
                return csv
                    .GetRecords<T>()
                    .ToList();
            }
        }
    }
}
