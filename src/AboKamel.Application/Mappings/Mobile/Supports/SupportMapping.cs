using Capsula.Application.Dtos.Mobile.Supports;
using Capsula.Domain.Entities.Supports;
using Services.Application.Mappings;

namespace Capsula.Application.Mappings.Mobile.Supports;

public static class SupportMapping
{
    public static void AddSupportMapping(this MappingProfiles map)
    {
        map.CreateMap<SupportRequestDto, Support>();
        map.CreateMap<Support, SupportResponseDto>();
        map.CreateMap<Support, SupportDetailedResponseDto>();
    }
}
