using AutoMapper;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Categories;
using Capsula.Domain.Entities.Categories;

namespace Capsula.Application.Mappings.Resolvers.Categories;

public class CategoryDetailsImageGenerateResolver : IValueResolver<Category, CategoryDetailedResponseDto, string?>
{
    private readonly IImageService _imageService;

    public CategoryDetailsImageGenerateResolver(IImageService imageService)
    {
        _imageService = imageService;
    }

    public string? Resolve(Category source, CategoryDetailedResponseDto destination, string? destMember, ResolutionContext context)
    {
        return _imageService.GenerateUrl(source.ImagePath);
    }
}