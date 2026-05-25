using AboKamel.Application.Dtos.Dashboard.SellingUnits;
using AboKamel.Domain.Entities.SellingUnits;
using Services.Application.Mappings;

namespace AboKamel.Application.Mappings.Dashboard.SellingUnits;

public static class SellingUnitMapping
{
    public static void AddSellingUnitMapping(this MappingProfiles map)
    {
        map.CreateMap<SellingUnitRequestDto, SellingUnit>();
        map.CreateMap<SellingUnit, SellingUnitResponseDto>();
    }
}