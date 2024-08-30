using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace CLDV_B_POE.Properties.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(configuration["AzureStorage:ConnectionString"]);
        }

        public async Task UploadBlobAsync(string blobst10332707, string blobName, Stream content)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(blobst10332707);
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(content, true);
        }
    }
}
