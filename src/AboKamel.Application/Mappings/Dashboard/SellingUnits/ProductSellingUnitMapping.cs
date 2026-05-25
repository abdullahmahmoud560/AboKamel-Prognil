using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using AboKamel.Domain.Entities.SellingUnits;
using Services.Application.Mappings;

namespace AboKamel.Application.Mappings.Dashboard.SellingUnits;

public static class ProductSellingUnitMapping
{
    public static void AddProductSellingUnitMapping(this MappingProfiles map)
    {
        map.CreateMap<ProductSellingUnitRequestDto, ProductSellingUnit>();
        map.CreateMap<ProductSellingUnit, ProductSellingUnitResponseDto>();
    }
}