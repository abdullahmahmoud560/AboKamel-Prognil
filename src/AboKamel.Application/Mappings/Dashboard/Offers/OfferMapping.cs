using AboKamel.Application.Dtos.Dashboard.Offers;
using AboKamel.Domain.Entities.Offers;
using Services.Application.Mappings;

namespace AboKamel.Application.Mappings.Dashboard.Offers;

public static class OfferMapping
{
    public static void AddOfferMapping(this MappingProfiles map)
    {
        map.CreateMap<OfferRequestDto, Offer>();
        map.CreateMap<Offer, OfferResponseDto>();
        map.CreateMap<Offer, OfferDetailedResponseDto>();
    }
}
