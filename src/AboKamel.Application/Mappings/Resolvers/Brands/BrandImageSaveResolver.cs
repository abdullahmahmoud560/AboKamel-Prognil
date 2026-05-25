using AutoMapper;
using Capsula.Application.Contracts.Images;
using Capsula.Application.Dtos.Dashboard.Brands;
using Capsula.Domain.Entities.Brands;

namespace Capsula.Application.Mappings.Resolvers.Brands;

public class BrandImageSaveResolver : IValueResolver<BrandRequestDto, Brand, string?>
{
    private readonly IImageService _imageService;

    public BrandImageSaveResolver(IImageService imageService)
    {
        _imageService = imageService;
    }

    public string? Resolve(BrandRequestDto source, Brand destination, string? destMember, ResolutionContext context)
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
