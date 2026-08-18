using Icof.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Icof.Api.Controllers;

[ApiController]
[Route("api/images")]
public class ImageController : ControllerBase
{
    private static readonly string[] AllowedContentTypes = { "image/jpeg", "image/png", "image/webp" };
    
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

    private readonly IBlobStorageService _blobStorage;

    public ImageController(IBlobStorageService blobStorage)
    {
        _blobStorage = blobStorage;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("people")]
    [RequestSizeLimit(MaxFileSizeBytes)]
    public async Task<IActionResult> UploadPersonImage(IFormFile file, CancellationToken cancellationToken)
    {
        var validationError = Validate(file);
        if (validationError is not null)
        {
            return BadRequest(validationError);
        }

        var blobName = $"people/{Guid.NewGuid()}{Path.GetExtension(file!.FileName)}";

        await using var stream = file.OpenReadStream();
        await _blobStorage.UploadAsync(stream, blobName, file.ContentType, cancellationToken);

        return Ok(new
        {
            blobName,
            url = _blobStorage.GetPublicUrl(blobName)
        });
    }

    private static string? Validate(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            return "No file uploaded.";
        }

        if (file.Length > MaxFileSizeBytes)
        {
            return "File is too large. Maximum size is 5 MB.";
        }

        if (!AllowedContentTypes.Contains(file.ContentType))
        {
            return "Unsupported file type. Use JPEG, PNG or WebP.";
        }

        return null;
    }
}
