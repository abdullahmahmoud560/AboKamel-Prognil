using Capsula.Application.Dtos.Mobile.Carts;
using Capsula.Domain.Entities.Carts;
using Microsoft.AspNetCore.Http;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Mobile.Carts;

public interface ICartService : IApplicationService, IScopedService
{
    Task<ResultAbstract<CartDetailedResponseDto>> GetCustomerCartDetailsAsync(string? customerId = null);
    Task<ResultAbstract<bool>> AddPrescriptionImageToCartAsync(IFormFile ImageFile, string? customerId = null);
    Task<ResultAbstract<bool>> AddPrescriptionVoiceRecordToCartAsync(IFormFile VoiceFile, string? customerId = null);
    Task<Cart> InitializeCustomerCartAsync(string? customerId = null);
    Task<Cart> GetCustomerCartAsync(string? customerId = null);
    Task<bool> CartExistsAsync(int cartId);
}
