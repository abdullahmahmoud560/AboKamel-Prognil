using Capsula.Application.Dtos.Mobile.Addresses;
using Capsula.Domain.Entities.Addresses;
using Services.Application.Mappings;

namespace Capsula.Application.Mappings.Mobile.Addresses;

public static class AddressMapping
{
    public static void AddAddressMapping(this MappingProfiles map)
    {
        map.CreateMap<AddressRequestDto, Address>();
        map.CreateMap<AddressNotPrimaryRequestDto, Address>();
        map.CreateMap<Address, AddressResponseDto>();
    }
}
