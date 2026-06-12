using Capsula.Application.Dtos.Mobile.Addresses;
using Capsula.Domain.Entities.Addresses;
using Services.Application.Mappings;

public static class AddressMapping
{
    public static void AddAddressMapping(this MappingProfiles map)
    {
        map.CreateMap<AddressRequestDto, Address>();

        map.CreateMap<AddressNotPrimaryRequestDto, Address>()
           .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        map.CreateMap<Address, AddressResponseDto>();
    }
}