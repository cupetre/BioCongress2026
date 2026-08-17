using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace Icof.Api.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IOptions<BlobStorageOptions> options)
        {
            var settings = options.Value;

            if (string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                throw new InvalidOperationException(
                    "BlobStorage:ConnectionString is not configured. Set it with " +
                    "\"dotnet user-secrets set \\\"BlobStorage:ConnectionString\\\" \\\"<value>\\\"\" locally, " +
                    "or as an app secret in Azure.");
            }

            var serviceClient = new BlobServiceClient(settings.ConnectionString);
            _containerClient = serviceClient.GetBlobContainerClient(settings.ContainerName);

            // Public read access on the blobs themselves (not container listing) — fine for
            // public congress media like team photos, event banners and logos.
            _containerClient.CreateIfNotExists(PublicAccessType.Blob);
        }

        public async Task<string> UploadAsync(
            Stream content,
            string blobName,
            string contentType,
            CancellationToken cancellationToken = default)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(
                content,
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
                },
                cancellationToken);

            return blobName;
        }

        public async Task DeleteAsync(string blobName, CancellationToken cancellationToken = default)
        {
            await _containerClient.GetBlobClient(blobName).DeleteIfExistsAsync(cancellationToken: cancellationToken);
        }

        public string GetPublicUrl(string blobName)
        {
            return _containerClient.GetBlobClient(blobName).Uri.ToString();
        }
    }
}
