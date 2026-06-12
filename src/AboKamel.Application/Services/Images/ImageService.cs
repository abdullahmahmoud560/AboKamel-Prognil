using Capsula.Application.Contracts.Images;
using Capsula.Application.Exceptions.Images;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Capsula.Application.Services.Images;

public class ImageService : IImageService
{
    // Added ".pdf" to the allowed extensions list
    private static readonly List<string> AllowedExtensions = new()
    {
        ".jpg", ".jpeg", ".png", ".webp", // Images
        ".mp3", ".wav", ".aac",          // Audio
        ".mp4", ".mov", ".avi",          // Video
        ".pdf"                           // Documents
    };

    // Max file size set to 50MB
    private const long MaxAllowedSize = 50 * 1024 * 1024;

    // Default image used as a fallback
    private const string DefaultPath = "uploads/default.png";

    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ImageService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
    {
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
    }

    // Save the file to the physical server storage
    public async Task SaveImageAsync(string relativePath, IFormFile? file)
    {
        if (file == null) return;

        ValidateImage(file);

        var fullPath = Path.Combine(_environment.WebRootPath, relativePath);

        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
    }

    // Determine storage folder and generate a unique file name
    public string GetImageRelativePath(IFormFile? file)
    {
        if (file == null) return DefaultPath;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid()}{extension}";

        // Store images in 'images' folder, media/documents in 'uploads'
        var folder = file.ContentType.StartsWith("image/") ? "images" : "uploads";
        return Path.Combine(folder, fileName);
    }

    // Validate file extension and size
    public void ValidateImage(IFormFile? file)
    {
        if (file == null) throw new ImageValidationException("File is required.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            throw new ImageValidationException("Invalid file extension.");

        if (file.Length > MaxAllowedSize)
            throw new ImageValidationException("File size exceeds the allowed limit.");
    }

    // Delete a file from the server
    public void DeleteImage(string? path)
    {
        if (string.IsNullOrWhiteSpace(path) || path == DefaultPath) return;

        var fullPath = Path.Combine(_environment.WebRootPath, path.TrimStart('/', '\\'));
        if (File.Exists(fullPath)) File.Delete(fullPath);
    }

    // Generate a public URL for the file
    public string? GenerateUrl(string? path)
    {
        if (string.IsNullOrEmpty(path)) path = DefaultPath;
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null) return path;

        var normalizedPath = path.Replace("\\", "/").TrimStart('/');
        return $"{request.Scheme}://{request.Host}/{normalizedPath}";
    }

    // Convert public URL back to a relative path
    public string ExtractImagePath(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return string.Empty;
        return new Uri(url).AbsolutePath.TrimStart('/');
    }
}