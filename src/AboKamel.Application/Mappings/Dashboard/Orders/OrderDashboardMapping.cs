using AboKamel.Application.Dtos.Dashboard.Orders;
using Capsula.Domain.Entities.Orders;
using Services.Application.Mappings;

namespace AboKamel.Application.Mappings.Dashboard.Orders;

public static class OrderDashboardMapping
{
    public static void AddOrderDashboardMapping(this MappingProfiles map)
    {
        map.CreateMap<Order, OrderResponseDto>()
            .ForMember(dest => dest.CustomerName, src => src.MapFrom(src => src.Customer.FullName));
        map.CreateMap<OrderItem, OrderItemResponseDto>();
    }
}