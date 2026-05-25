using AutoMapper;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Domain.Entities.Products;

namespace Capsula.Application.Mappings.Resolvers.Products;

public class ProductImageGenerateResolver : IValueResolver<Product, Dtos.Dashboard.Products.ProductResponseDto, string?>
{
    private readonly IImageService _imageService;

    public ProductImageGenerateResolver(IImageService imageService)
    {
        _imageService = imageService;
    }

    public string? Resolve(Product source, Dtos.Dashboard.Products.ProductResponseDto destination, string? destMember, ResolutionContext context)
    {
        return _imageService.GenerateUrl(source.ImagePath);
    }
}
