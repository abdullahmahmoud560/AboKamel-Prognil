using AutoMapper;
using Capsula.Application.Contracts.Mobile.Addresses;
using Capsula.Application.Dtos.Mobile.Addresses;
using Capsula.Domain.Entities.Addresses;
using Capsula.Domain.Repositories.Addresses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Core.Results;
using System.Security.Claims;

namespace Capsula.Application.Services.Mobile.Addresses;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Address> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AddressService(IAddressRepository addressRepository, IMapper mapper, ILogger<Address> logger, IHttpContextAccessor httpContextAccessor)
    {
        _addressRepository = addressRepository;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    private string GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }
        return userId;
    }

    public async Task<ResultAbstract<AddressResponseDto>> CreateCustomerAddressAsync(AddressRequestDto request, string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();
        if(request.IsPrimary)
        {
            var primaryAddress = await _addressRepository.GetPrimaryAddressAsync(currentUserId);

            if (primaryAddress is not null)
            {
                primaryAddress.IsPrimary = false;
                var isAddressUpdated = await _addressRepository.EditAsync(primaryAddress);

                if (!isAddressUpdated)
                {
                    _logger.LogError("Could not update primary address");
                    return Result.Error("Could not update primary address");
                }
            }
        }

        request.CustomerId = currentUserId;
        var address = _mapper.Map<Address>(request);

        var isAddressAdded = await _addressRepository.AddAsync(address);

        if (!isAddressAdded)
        {
            _logger.LogError("Could not add address");
            return Result.Error("Could not add address");
        }

        return Result.Success(_mapper.Map<AddressResponseDto>(address));
    }

    public async Task<ResultAbstract<AddressResponseDto>> UpdateCustomerAddressAsync(AddressNotPrimaryRequestDto request, int addressId, string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();
        var address = await _addressRepository.GetCustomerAddressById(currentUserId, addressId);

        if(address is null)
        {
            _logger.LogError("Could not find address");
            return Result.Error("Could not find address");
        }

        request.CustomerId = currentUserId;
        _mapper.Map(request, address);

        var isAddressUpdated = await _addressRepository.EditAsync(address);

        if (!isAddressUpdated)
        {
            _logger.LogError("Could not update address");
            return Result.Error("Could not update address");
        }

        return Result.Success(_mapper.Map<AddressResponseDto>(address));
    }

    public async Task<ResultAbstract<AddressResponseDto>> DeleteCustomerAddressAsync(int addressId, string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();
        var address = await _addressRepository.GetCustomerAddressById(currentUserId, addressId);

        if (address is null)
        {
            _logger.LogError("Could not find address");
            return Result.Error("Could not find address");
        }

        var isAddressDeleted = await _addressRepository.DeleteAsync(address);

        if (!isAddressDeleted)
        {
            _logger.LogError("Could not delete address");
            return Result.Error("Could not delete address");
        }

        return Result.Success(_mapper.Map<AddressResponseDto>(address));
    }

    public async Task<ResultAbstract<IEnumerable<AddressResponseDto>>> GetCustomerAddressesAsync(string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();
        var addresses = await _addressRepository.GetCustomerAddressesAsync(currentUserId);
        return Result.Success(_mapper.Map<IEnumerable<AddressResponseDto>>(addresses));
    }

    public async Task<ResultAbstract<AddressResponseDto>> GetPrimaryAddressAsync(string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();
        var primaryAddress = await _addressRepository.GetPrimaryAddressAsync(currentUserId);
        return Result.Success(_mapper.Map<AddressResponseDto>(primaryAddress));
    }

    public async Task<ResultAbstract<AddressResponseDto>> MarkAddressAsPrimaryAsync(int addressId, string? customerId = null)
    {
        var currentUserId = customerId ?? GetCurrentUserId();
        var primaryAddress = await _addressRepository.GetPrimaryAddressAsync(currentUserId);

        var addressToBePrimary = await _addressRepository.GetByIdAsync(addressId);

        if(addressToBePrimary is null)
        {
            _logger.LogWarning("Could not find address");
            return Result.Error("Address was not found");
        }

        if (primaryAddress is not null)
        {
            primaryAddress.IsPrimary = false;
            var isCurrentAddressUpdated = await _addressRepository.EditAsync(primaryAddress);

            if (!isCurrentAddressUpdated)
            {
                _logger.LogError("Could not update current primary address");
                return Result.Error("Could not update current primary address");
            }
        }

        addressToBePrimary.IsPrimary = true;
        var isAddressUpdated = await _addressRepository.EditAsync(addressToBePrimary);

        if (!isAddressUpdated)
        {
            _logger.LogError("Could not update address to primary");
            return Result.Error("Could not update address to primary");
        }

        return Result.Success(_mapper.Map<AddressResponseDto>(addressToBePrimary));
    }
}
