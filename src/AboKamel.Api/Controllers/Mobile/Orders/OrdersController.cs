using AboKamel.Application.Contracts.Mobile.Orders;
using AboKamel.Application.Dtos.Dashboard.Orders;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Domain.Entities.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using Services.Core.Results;
using System.Security.Claims;

namespace AboKamel.Api.Controllers.Mobile.Orders;

public class OrdersController : MobileBaseController
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("CreateCustomerOrder")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<OrderResponseDto>>> CreateOrderAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _orderService.CreateOrderAsync(userId);
    }

    [HttpGet("GetCustomerOrders")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<List<OrderResponseDto>>>> GetCustomerOrdersAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _orderService.GetCustomerOrdersAsync(userId);
    }

    [HttpGet("GetCustomerOrderById/orderId/{orderId}")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<OrderResponseDto>>> GetCustomerOrderByIdAsync(int orderId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _orderService.GetCustomerOrderOrderByIdAsync(userId, orderId);
    }

    [HttpGet("GetCustomerLastOrderedProducts")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<List<ProductResponseDto>>>> GetCustomerLastOrderedProductsAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _orderService.GetCustomerLastOrderedProductsAsync(userId);
    }
}