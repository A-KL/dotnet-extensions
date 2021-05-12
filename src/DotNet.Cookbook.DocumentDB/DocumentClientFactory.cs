using System;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DotNet.Cookbook.DocumentDB
{
    public class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly DocumentDbSettings _settings;

        public DocumentClientFactory(DocumentDbSettings settings)
        {
            _settings = settings;
        }

        public IDocumentClient Create()
        {
            return new DocumentClient(new Uri(_settings.Uri), _settings.PrimaryKey);
        }
    }
}
