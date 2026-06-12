using Capsula.Application.Dtos.Mobile.Addresses;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Mobile.Addresses;

public interface IAddressService : IApplicationService, IScopedService
{
    Task<ResultAbstract<AddressResponseDto>> CreateCustomerAddressAsync(AddressRequestDto request, string? customerId = null);
    Task<ResultAbstract<AddressResponseDto>> UpdateCustomerAddressAsync(AddressNotPrimaryRequestDto request, int addressId, string? customerId = null);
    Task<ResultAbstract<AddressResponseDto>> DeleteCustomerAddressAsync(int addressId, string? customerId = null);
    Task<ResultAbstract<IEnumerable<AddressResponseDto>>> GetCustomerAddressesAsync(string? customerId = null);
    Task<ResultAbstract<AddressResponseDto>> GetPrimaryAddressAsync(string? customerId = null);
    Task<ResultAbstract<AddressResponseDto>> MarkAddressAsPrimaryAsync(int addressId, string? customerId = null);
}