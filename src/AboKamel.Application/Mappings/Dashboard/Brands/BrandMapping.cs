using Capsula.Application.Dtos.Dashboard.Brands;
using Capsula.Application.Mappings.Resolvers.Brands;
using Capsula.Domain.Entities.Brands;
using Services.Application.Mappings;

namespace Capsula.Application.Mappings.Dashboard.Brands;

public static class BrandMapping
{
    public static void AddBrandMapping(this MappingProfiles map)
    {
        map.CreateMap<BrandRequestDto, Brand>().ForMember(dest => dest.ImagePath, src => src.MapFrom<BrandImageSaveResolver>());

        map.CreateMap<Brand, BrandResponseDto>().ForMember(dest => dest.ImageUrl, src => src.MapFrom<BrandImageGenerateResolver>());

        map.CreateMap<Brand, BrandDetailedResponseDto>().ForMember(dest => dest.ImageUrl, src => src.MapFrom<BrandDetailsImageGenerateResolver>());
    }
}
