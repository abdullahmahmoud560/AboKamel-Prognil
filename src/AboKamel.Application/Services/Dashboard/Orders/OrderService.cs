using AboKamel.Application.Contracts.Dashboard.Orders;
using AboKamel.Application.Dtos.Dashboard.Orders;
using AboKamel.Core.Enums;
using AboKamel.Domain.Repositories.Orders;
using AutoMapper;
using Services.Core.Results;
using Services.Infrastructure.DbContexts;

namespace AboKamel.Application.Services.Dashboard.Orders;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly CapsulaDbContext _context;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper, CapsulaDbContext context)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<ResultAbstract<OrderResponseDto>> AddDiscountAsync(int orderId, decimal discount)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if (order is null)
        {
            return Result.Error("Could not find order.");
        }

        order.Discount = discount;

        await _orderRepository.EditAsync(order);

        var orderResponse = _mapper.Map<OrderResponseDto>(order);

        return Result.Success(orderResponse);
    }

    public async Task<ResultAbstract<List<OrderResponseDto>>> GetAllOrdersAsync()
    {
        var order = await _orderRepository.GetOrdersAsync();

        return Result.Success(_mapper.Map<List<OrderResponseDto>>(order));
    }

    public async Task<ResultAbstract<OrderResponseDto>> GetOrderByIdAsync(int orderId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        var orderDto = _mapper.Map<OrderResponseDto>(order);
        return Result.Success(orderDto);
    }

    public async Task<ResultAbstract<List<OrderResponseDto>>> GetOrdersByStatusAsync(OrderStatus status)
    {
        var orders = await _orderRepository.GetOrdersByStatusAsync(status);

        return Result.Success(_mapper.Map<List<OrderResponseDto>>(orders));
    }

    public async Task<ResultAbstract<OrderResponseDto>> UpdateOrderItemQuantityAsync(int orderId, int productId, int quantity)
    {
        var orderItem = await _orderRepository.GetOrderItemByOrderIdAndProductId(orderId, productId);

        if (orderItem is null)
        {
            return Result.Error("Could not find order item.");
        }

        string message;

        if (quantity == 0)
        {
            _context.OrderItems.Remove(orderItem);
            message = $"Successfully deleted order item.";
        }
        else
        {
            orderItem.Quantity = quantity;
            message = $"Successfully updated order item quantity to {quantity}.";
        }

        await _context.SaveChangesAsync();

        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        var orderDto = _mapper.Map<OrderResponseDto>(order);
        return Result.Success(orderDto, message);
    }

    public async Task<ResultAbstract<OrderResponseDto>> UpdateOrderStatusAsync(int orderId, OrderStatus status, DateOnly? arrivalDate, string? notes)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);

        if(order is null)
        {
            return Result.Error("Could not find order.");
        }

        if (status == OrderStatus.Approved)
        {
            order.ArrivalDate = arrivalDate;
            order.Notes = notes;
        }

        order.Status = status;

        await _orderRepository.EditAsync(order);

        var orderResponse = _mapper.Map<OrderResponseDto>(order);

        return Result.Success(orderResponse);
    }
}
