using AboKamel.Application.Dtos.Dashboard.Areas;
using AboKamel.Domain.Entities.Areas;
using Services.Application.Mappings;

namespace AboKamel.Application.Mappings.Dashboard.Areas;

public static class AreaMapping
{
    public static void AddAreaMapping(this MappingProfiles map)
    {
        map.CreateMap<AreaRequestDto, Area>();
        map.CreateMap<Area, AreaResponseDto>();
    }
}