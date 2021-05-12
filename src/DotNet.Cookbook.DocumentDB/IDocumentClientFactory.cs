
using Microsoft.Azure.Documents;

namespace DotNet.Cookbook.DocumentDB
{
    public interface IDocumentClientFactory
    {
        IDocumentClient Create();
    }
}
