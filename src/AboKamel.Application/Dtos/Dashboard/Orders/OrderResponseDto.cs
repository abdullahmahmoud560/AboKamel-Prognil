using AboKamel.Core.Enums;
using Capsula.Application.Dtos;
using Capsula.Domain.Entities.Orders;
using Capsula.Domain.Entities.Users.Customers;
using System.Text.Json.Serialization;

namespace AboKamel.Application.Dtos.Dashboard.Orders;

public class OrderResponseDto : BaseResponseDto<int>
{
    public string CustomerId { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;
    public string DetailedAddress { get; set; } = string.Empty;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;

    public decimal? Discount { get; set; }

    public DateOnly? ArrivalDate { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; }
    public string? Notes { get; set; }

    public ICollection<OrderItemResponseDto> Items { get; set; } = new List<OrderItemResponseDto>();
}
