using AboKamel.Application.Dtos.Dashboard.Roles;
using Capsula.Application.Dtos.Authentication.Users.Customers;
using Capsula.Domain.Entities.Users.Customers;
using Microsoft.AspNetCore.Identity;
using Services.Application.Dtos.Authentication;
using Services.Domain.Entities.Users;

namespace Services.Application.Mappings;

public static class AuthMapping
{
    public static void AddAuthMapping(this MappingProfiles map)
    {
        map.CreateMap<ApplicationUser, LoginResponseDto>()
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id));

        map.CreateMap<ApplicationUser, BaseUserResponseDto>();


        map.CreateMap<CustomerRequestDto, Customer>();
        map.CreateMap<Customer, CustomerResponseDto>();

        map.CreateMap<IdentityRole, RoleResponseDto>();
    }
}
