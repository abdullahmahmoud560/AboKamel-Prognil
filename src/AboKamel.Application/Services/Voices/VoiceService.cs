using Capsula.Application.Contracts.Voices;
using Capsula.Application.Exceptions.Voices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Capsula.Application.Services.Voices;

public class VoiceService : IVoiceService
{
    private static readonly List<string> AllowedExtensions = new() { ".mp3", ".wav", ".ogg", ".m4a" };
    private const long MaxAllowedVoiceSize = 10 * 1024 * 1024; // 10 MB max
    private const string DefaultVoicePath = "voices/No_Voice.mp3";

    private readonly IWebHostEnvironment _environment;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VoiceService(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
    {
        _environment = environment;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Validates and saves a voice recording file. Returns relative path (e.g., "voices/abc.mp3").
    /// Returns default path if null.
    /// </summary>
    public async Task SaveVoiceAsync(string relativePath, IFormFile? voice)
    {
        if (relativePath != DefaultVoicePath)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, relativePath);

            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!); // Ensure folder exists

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await voice.CopyToAsync(stream);
            }
        }
    }

    public string GetVoiceRelativePath(IFormFile? voice)
    {
        if (voice == null)
            return DefaultVoicePath;

        var extension = Path.GetExtension(voice.FileName).ToLowerInvariant();
        var voiceName = $"{Guid.NewGuid()}{extension}";
        var relativePath = Path.Combine("voices", voiceName);

        return relativePath;
    }

    public void ValidateVoice(IFormFile? voice)
    {
        if (voice is not null)
        {
            var extension = Path.GetExtension(voice.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                throw new VoiceValidationException($"Invalid file extension '{extension}'. Allowed: {string.Join(", ", AllowedExtensions)}");

            if (voice.Length > MaxAllowedVoiceSize)
                throw new VoiceValidationException($"File size exceeds the allowed limit of {MaxAllowedVoiceSize / 1024 / 1024} MB.");
        }
    }

    /// <summary>
    /// Deletes a voice recording if it exists (ignores default).
    /// </summary>
    public void DeleteVoice(string? voicePath)
    {
        if (string.IsNullOrWhiteSpace(voicePath) || voicePath == DefaultVoicePath)
            return;

        var fullPath = Path.Combine(_environment.WebRootPath, voicePath.TrimStart('/', '\\'));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    /// <summary>
    /// Generates absolute URL from a relative voice path.
    /// </summary>
    public string? GenerateUrl(string? voicePath)
    {
        if (string.IsNullOrEmpty(voicePath))
        {
            voicePath = DefaultVoicePath;
        }

        var request = _httpContextAccessor.HttpContext?.Request;
        if (request == null)
            return voicePath; // fallback to relative

        var normalizedPath = voicePath.Replace("\\", "/").TrimStart('/');
        return $"{request.Scheme}://{request.Host}/{normalizedPath}";
    }

    public string ExtractVoicePath(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        var uri = new Uri(url);
        return uri.AbsolutePath.TrimStart('/');
    }
}
