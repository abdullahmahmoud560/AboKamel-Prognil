using Capsula.Application.Dtos.Mobile.Addresses;
using Services.Core.DependencyInjection;
using Services.Core.Results;

namespace Capsula.Application.Contracts.Mobile.Addresses;

public interface IAddressService : IApplicationService, IScopedService
{
    Task<ResultAbstract<AddressResponseDto>> CreateCustomerAddressAsync(AddressRequestDto request, string customerId);
    Task<ResultAbstract<AddressResponseDto>> UpdateCustomerAddressAsync(AddressNotPrimaryRequestDto request, string customerId, int addressId);
    Task<ResultAbstract<AddressResponseDto>> DeleteCustomerAddressAsync(string customerId, int addressId);
    Task<ResultAbstract<IEnumerable<AddressResponseDto>>> GetCustomerAddressesAsync(string customerId);
    Task<ResultAbstract<AddressResponseDto>> GetPrimaryAddressAsync(string customerId);
    Task<ResultAbstract<AddressResponseDto>> MarkAddressAsPrimaryAsync(string customerId, int addressId);
}