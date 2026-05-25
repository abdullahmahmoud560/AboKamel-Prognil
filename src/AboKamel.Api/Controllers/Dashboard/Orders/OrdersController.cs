using AboKamel.Application.Contracts.Dashboard.Orders;
using AboKamel.Application.Dtos.Dashboard.Orders;
using AboKamel.Core.Enums;
using Capsula.Api.Controllers.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Results;

namespace AboKamel.Api.Controllers.Dashboard.Orders;

public class OrdersController : DashboardBaseController
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ResultAbstract<OrderResponseDto>>> GetAllOrdersAsynnc()
    {
        var result = await _orderService.GetAllOrdersAsync();

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("GetOrderById/{orderId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ResultAbstract<OrderResponseDto>>> GetOrderByIdAsync(int orderId)
    {
        var result = await _orderService.GetOrderByIdAsync(orderId);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpPatch("AddOrderDiscount/{orderId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ResultAbstract<OrderResponseDto>>> AddOrderDiscount(int orderId, decimal discount)
    {
        var result = await _orderService.AddDiscountAsync(orderId, discount);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("GetOrderByStatus/{status}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<ActionResult<ResultAbstract<OrderResponseDto>>> GetOrdersByStatus(OrderStatus status)
    {
        var result = await _orderService.GetOrdersByStatusAsync(status);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("UpdateOrderStatus/order/{orderId}/status/{status}/notes/{notes}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus status, DateOnly? arrivalDate, string? notes)
    {
        var result = await _orderService.UpdateOrderStatusAsync(orderId, status, arrivalDate, notes);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("UpdateOrderItemQuantity/order/{orderId}/product/{productId}/quantity/{quantity}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> UpdateOrderItemQuantityAsync(int orderId, int productId, int quantity)
    {
        var result = await _orderService.UpdateOrderItemQuantityAsync(orderId, productId, quantity);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }

    [HttpDelete("DeleteOrderItem/order/{orderId}/product/{productId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> DeleteOrderItemAsync(int orderId, int productId)
    {
        var result = await _orderService.UpdateOrderItemQuantityAsync(orderId, productId, 0);

        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
}
