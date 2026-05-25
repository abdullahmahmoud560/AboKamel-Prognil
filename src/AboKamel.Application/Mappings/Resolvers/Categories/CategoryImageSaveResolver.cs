using AutoMapper;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Categories;
using Capsula.Domain.Entities.Categories;

namespace Capsula.Application.Mappings.Resolvers.Categoriesp;

public class CategoryImageSaveResolver : IValueResolver<CategoryRequestDto, Category, string?>
{
    private readonly IImageService _imageService;

    public CategoryImageSaveResolver(IImageService imageService)
    {
        _imageService = imageService;
    }

    public string? Resolve(CategoryRequestDto source, Category destination, string? destMember, ResolutionContext context)
    {
        if (source.ImageFile is not null)
        {
            return _imageService.GetImageRelativePath(source.ImageFile);
        }
        else
        {
            return destination.ImagePath;
        }
    }
}