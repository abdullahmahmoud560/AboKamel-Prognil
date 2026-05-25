using Capsula.Application.Contracts.Images;
using Capsula.Application.Exceptions.Images;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Capsula.Application.Services.Images;

public class ImageService : IImageService
{
    private static readonly List<string> AllowedExtensions = new() { ".png", ".jpg", ".jpeg", ".webp" };
    private const long MaxAllowedImageSize = 2 * 1024 * 1024; // 2 MB
    private const string DefaultImagePath = "images/No_Image.png";

    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ImageService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
    {
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Validates and saves an uploaded image. Returns relative path (e.g., "images/abc.png").
    /// Returns default image path if null.
    /// </summary>
    public async Task SaveImageAsync(string relativePath, IFormFile? image)
    {
        if(relativePath != DefaultImagePath)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!); // Ensure folder exists

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
        }
    }

    public string GetImageRelativePath(IFormFile? image)
    {
        if (image == null)
            return DefaultImagePath;

        var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
        var imgName = $"{Guid.NewGuid()}{extension}";
        var relativePath = Path.Combine("images", imgName);

        return relativePath;
    }

    public void ValidateImage(IFormFile? image)
    {
        if(image is not null)
        {
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new ImageValidationException($"Invalid file extension '{extension}'. Allowed: {string.Join(", ", AllowedExtensions)}");

            if (image.Length > MaxAllowedImageSize)
                throw new ImageValidationException($"File size exceeds the allowed limit of {MaxAllowedImageSize / 1024 / 1024} MB.");
        }
    }

    /// <summary>
    /// Deletes an image if it exists (ignores default image).
    /// </summary>
    public void DeleteImage(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath) || imagePath == DefaultImagePath)
            return;

        var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/', '\\'));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    /// <summary>
    /// Generates absolute URL from a relative image path.
    /// </summary>
    public string? GenerateUrl(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            imagePath = DefaultImagePath;
        }

        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
            return imagePath; // fallback to relative

        // Normalize path (replace backslashes with forward slashes)
        var normalizedPath = imagePath.Replace("\\", "/").TrimStart('/');

        return $"{request.Scheme}://{request.Host}/{normalizedPath}";
    }

    public string ExtractImagePath(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        var uri = new Uri(url);
        return uri.AbsolutePath.TrimStart('/');
    }
}
