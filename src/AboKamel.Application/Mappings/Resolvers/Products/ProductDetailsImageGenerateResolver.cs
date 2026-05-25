using AutoMapper;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Domain.Entities.Products;

namespace Capsula.Application.Mappings.Resolvers.Products;

public class ProductDetailsImageGenerateResolver : IValueResolver<Product, ProductDetailedResponseDto, string?>
{
    private readonly IImageService _imageService;

    public ProductDetailsImageGenerateResolver(IImageService imageService)
    {
        _imageService = imageService;
    }

    public string? Resolve(Product source, ProductDetailedResponseDto destination, string? destMember, ResolutionContext context)
    {
        return _imageService.GenerateUrl(source.ImagePath);
    }
}
