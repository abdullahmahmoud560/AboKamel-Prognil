using Microsoft.AspNetCore.Http;
using Services.Core.DependencyInjection;

namespace Capsula.Application.Contracts.Images;

public interface IImageService : IApplicationService, IScopedService
{
    Task SaveImageAsync(string relativePath, IFormFile? image);
    string GetImageRelativePath(IFormFile? image);
    void DeleteImage(string? imageUrl);
    string? GenerateUrl(string? imageUrl);
    string ExtractImagePath(string url);
    void ValidateImage(IFormFile? image);
}
