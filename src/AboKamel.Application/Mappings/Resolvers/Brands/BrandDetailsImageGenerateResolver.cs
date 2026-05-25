using AutoMapper;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Brands;
using Capsula.Domain.Entities.Brands;

namespace Capsula.Application.Mappings.Resolvers.Brands;

public class BrandDetailsImageGenerateResolver : IValueResolver<Brand, BrandDetailedResponseDto, string?>
{
    private readonly IImageService _imageService;

    public BrandDetailsImageGenerateResolver(IImageService imageService)
    {
        _imageService = imageService;
    }

    public string? Resolve(Brand source, BrandDetailedResponseDto destination, string? destMember, ResolutionContext context)
    {
        return _imageService.GenerateUrl(source.ImagePath);
    }
}