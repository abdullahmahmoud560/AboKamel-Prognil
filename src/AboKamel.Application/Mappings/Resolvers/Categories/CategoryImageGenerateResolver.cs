using AutoMapper;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Categories;
using Capsula.Domain.Entities.Categories;

namespace Capsula.Application.Mappings.Resolvers.Categories;

public class CategoryImageGenerateResolver : IValueResolver<Category, CategoryResponseDto, string?>
{
    private readonly IImageService _imageService;

    public CategoryImageGenerateResolver(IImageService imageService)
    {
        _imageService = imageService;
    }

    public string? Resolve(Category source, CategoryResponseDto destination, string? destMember, ResolutionContext context)
    {
        return _imageService.GenerateUrl(source.ImagePath);
    }
}
