using AboKamel.Core.Enums;

namespace AboKamel.Core.Dtos.Analysis;

public class OrderStatusCountDto
{
    public OrderStatus Status { get; set; }
    public int Count { get; set; }
}