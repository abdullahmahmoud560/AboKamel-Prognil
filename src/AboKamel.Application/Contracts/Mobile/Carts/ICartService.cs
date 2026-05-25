using Capsula.Application.Dtos.Mobile.Carts;
using Capsula.Domain.Entities.Carts;
using Microsoft.AspNetCore.Http;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Mobile.Carts;

public interface ICartService : IApplicationService, IScopedService
{
    Task<ResultAbstract<CartDetailedResponseDto>> GetCustomerCartDetailsAsync(string customerId);
    Task<ResultAbstract<bool>> AddPrescriptionImageToCartAsync(string customerId, IFormFile ImageFile);
    Task<ResultAbstract<bool>> AddPrescriptionVoiceRecordToCartAsync(string customerId, IFormFile VoiceFile);
    Task<Cart> InitializeCustomerCartAsync(string customerId);
    Task<Cart> GetCustomerCartAsync(string customerId);
    Task<bool> CartExistsAsync(int cartId);
}
