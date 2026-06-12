using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Application.Mappings.Resolvers.Products;
using Capsula.Domain.Entities.Products;
using Services.Application.Mappings;

namespace Capsula.Application.Mappings.Dashboard.Products;

public static class ProductMapping
{
    public static void AddProductMapping(this MappingProfiles map)
    {
        map.CreateMap<ProductRequestDto, Product>()
            .ForMember(
                dest => dest.AreaId,
                opt => opt.MapFrom(src => src.AreaId == 0 ? (int?)null : src.AreaId)
            )
            .ForMember(dest => dest.ProductSellingUnits, opt => opt.Ignore())
            .ForMember<string>(dest => dest.ImagePath!, src => src.MapFrom<ProductImageSaveResolver>());

        map.CreateMap<Product, ProductResponseDto>().ForMember(dest => dest.ImageUrl, src => src.MapFrom<ProductImageGenerateResolver>());

        map.CreateMap<Product, ProductDetailedResponseDto>().ForMember<string>(dest => dest.ImageUrl!, src => src.MapFrom<ProductDetailsImageGenerateResolver>());
    }
}
