namespace Icof.Api.Services
{
    public interface IBlobStorageService
    {
        /// <summary>
        /// Uploads a file to the media container and returns the blob name that was used
        /// (this is the value that should be stored on entities, e.g. TeamMember.PhotoBlobName).
        /// </summary>
        Task<string> UploadAsync(
            Stream content,
            string blobName,
            string contentType,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(string blobName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Resolves a stored blob name into the full public URL the frontend can use directly in an &lt;img&gt; src.
        /// </summary>
        string GetPublicUrl(string blobName);
    }
}
