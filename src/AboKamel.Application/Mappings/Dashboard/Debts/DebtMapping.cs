using AboKamel.Application.Dtos.Dashboard.Debts;
using AboKamel.Domain.Entities.Debts;
using Services.Application.Mappings;

namespace AboKamel.Application.Mappings.Dashboard.Debts;

public static class DebtMapping
{
    public static void AddDebtMapping(this MappingProfiles map)
    {
        map.CreateMap<DebtRequestDto, Debt>();
        map.CreateMap<Debt, DebtResponseDto>()
            .ForMember(dest => dest.FullName, src => src.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(src => src.Customer.Addresses.FirstOrDefault().PhoneNumber));
    }
}
