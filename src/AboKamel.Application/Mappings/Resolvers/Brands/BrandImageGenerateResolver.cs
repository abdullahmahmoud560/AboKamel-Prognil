using AutoMapper;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Brands;
using Capsula.Domain.Entities.Brands;

namespace Capsula.Application.Mappings.Resolvers.Brands;

public class BrandImageGenerateResolver : IValueResolver<Brand, BrandResponseDto, string?>
{
    private readonly IImageService _imageService;

    public BrandImageGenerateResolver(IImageService imageService)
    {
        _imageService = imageService;
    }

    public string? Resolve(Brand source, BrandResponseDto destination, string? destMember, ResolutionContext context)
    {
        return _imageService.GenerateUrl(source.ImagePath);
    }
}