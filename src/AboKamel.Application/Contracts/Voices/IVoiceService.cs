using Microsoft.AspNetCore.Http;
using Services.Core.DependencyInjection;

namespace Capsula.Application.Contracts.Voices;

public interface IVoiceService : IApplicationService, IScopedService
{
    Task SaveVoiceAsync(string relativePath, IFormFile? voice);
    string GetVoiceRelativePath(IFormFile? voice);
    void DeleteVoice(string? voiceUrl);
    string? GenerateUrl(string? voiceUrl);
    string ExtractVoicePath(string url);
    void ValidateVoice(IFormFile? voice);
}
