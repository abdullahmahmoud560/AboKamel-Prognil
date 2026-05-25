using AutoMapper;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Domain.Entities.Products;

namespace Capsula.Application.Mappings.Resolvers.Products;

public class ProductImageSaveResolver : IValueResolver<ProductRequestDto, Product, string?>
{
    private readonly IImageService _imageService;

    public ProductImageSaveResolver(IImageService imageService)
    {
        _imageService = imageService;
    }

    public string? Resolve(ProductRequestDto source, Product destination, string? destMember, ResolutionContext context)
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
