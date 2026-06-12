using AboKamel.Application.Contracts.Mobile.Orders;
using AboKamel.Application.Dtos.Dashboard.Orders;
using Capsula.Application.Dtos.Dashboard.Products;
using Capsula.Domain.Entities.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Api.Controllers.Mobile;
using Services.Core.Results;

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
        return await _orderService.CreateOrderAsync();
    }

    [HttpGet("GetCustomerOrders")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<List<OrderResponseDto>>>> GetCustomerOrdersAsync()
    {
        return await _orderService.GetCustomerOrdersAsync();
    }

    [HttpGet("GetCustomerOrderById/orderId/{orderId}")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<OrderResponseDto>>> GetCustomerOrderByIdAsync(int orderId)
    {
        return await _orderService.GetCustomerOrderOrderByIdAsync(orderId);
    }

    [HttpGet("GetCustomerLastOrderedProducts")]
    [Authorize(Roles = "Customer")]
    public async Task<ActionResult<ResultAbstract<List<ProductResponseDto>>>> GetCustomerLastOrderedProductsAsync()
    {
        return await _orderService.GetCustomerLastOrderedProductsAsync();
    }
}
