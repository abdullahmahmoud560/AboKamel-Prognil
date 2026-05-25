using AutoMapper;
using Capsula.Application.Contracts.Mobile.Addresses;
using Capsula.Application.Dtos.Mobile.Addresses;
using Capsula.Domain.Entities.Addresses;
using Capsula.Domain.Repositories.Addresses;
using Microsoft.Extensions.Logging;
using Services.Core.Results;

namespace Capsula.Application.Services.Mobile.Addresses;

public class AddressService : IAddressService
{
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<Address> _logger;

    public AddressService(IAddressRepository addressRepository, IMapper mapper, ILogger<Address> logger)
    {
        _addressRepository = addressRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ResultAbstract<AddressResponseDto>> CreateCustomerAddressAsync(AddressRequestDto request, string customerId)
    {
        if(request.IsPrimary)
        {
            var primaryAddress = await _addressRepository.GetPrimaryAddressAsync(customerId);

            if (primaryAddress is not null)
            {
                primaryAddress.IsPrimary = false;
                var isAddressUpdated = await _addressRepository.EditAsync(primaryAddress);

                if (!isAddressUpdated)
                {
                    _logger.LogError("Could not update primary address");
                    return Result.Error("Could not update primary address.");
                }
            }
        }

        request.CustomerId = customerId;
        var address = _mapper.Map<Address>(request);

        var isAddressAdded = await _addressRepository.AddAsync(address);

        if (!isAddressAdded)
        {
            _logger.LogError("Could not add address");
            return Result.Error("Could not add address.");
        }

        return Result.Success(_mapper.Map<AddressResponseDto>(address));
    }

    public async Task<ResultAbstract<AddressResponseDto>> UpdateCustomerAddressAsync(AddressNotPrimaryRequestDto request, string customerId, int addressId)
    {
        var address = await _addressRepository.GetCustomerAddressById(customerId, addressId);

        if(address is null)
        {
            _logger.LogError("Could not find address");
            return Result.Error("Could not find address.");
        }

        request.CustomerId = customerId;
        _mapper.Map(request, address);

        var isAddressUpdated = await _addressRepository.EditAsync(address);

        if (!isAddressUpdated)
        {
            _logger.LogError("Could not update address");
            return Result.Error("Could not update address.");
        }

        return Result.Success(_mapper.Map<AddressResponseDto>(address));
    }

    public async Task<ResultAbstract<AddressResponseDto>> DeleteCustomerAddressAsync(string customerId, int addressId)
    {
        var address = await _addressRepository.GetCustomerAddressById(customerId, addressId);

        if (address is null)
        {
            _logger.LogError("Could not find address");
            return Result.Error("Could not find address.");
        }

        var isAddressDeleted = await _addressRepository.DeleteAsync(address);

        if (!isAddressDeleted)
        {
            _logger.LogError("Could not delete address");
            return Result.Error("Could not delete address.");
        }

        return Result.Success(_mapper.Map<AddressResponseDto>(address));
    }

    public async Task<ResultAbstract<IEnumerable<AddressResponseDto>>> GetCustomerAddressesAsync(string customerId)
    {
        var addresses = await _addressRepository.GetCustomerAddressesAsync(customerId);
        return Result.Success(_mapper.Map<IEnumerable<AddressResponseDto>>(addresses));
    }

    public async Task<ResultAbstract<AddressResponseDto>> GetPrimaryAddressAsync(string customerId)
    {
        var primaryAddress = await _addressRepository.GetPrimaryAddressAsync(customerId);
        return Result.Success(_mapper.Map<AddressResponseDto>(primaryAddress));
    }

    public async Task<ResultAbstract<AddressResponseDto>> MarkAddressAsPrimaryAsync(string customerId, int addressId)
    {
        var primaryAddress = await _addressRepository.GetPrimaryAddressAsync(customerId);

        var addressToBePrimary = await _addressRepository.GetByIdAsync(addressId);

        if(addressToBePrimary is null)
        {
            _logger.LogWarning("Could not find address");
            return Result.Error("Address was not found.");
        }

        if (primaryAddress is not null)
        {
            primaryAddress.IsPrimary = false;
            var isCurrentAddressUpdated = await _addressRepository.EditAsync(primaryAddress);

            if (!isCurrentAddressUpdated)
            {
                _logger.LogError("Could not update current primary address");
                return Result.Error("Could not update current primary address.");
            }
        }

        addressToBePrimary.IsPrimary = true;
        var isAddressUpdated = await _addressRepository.EditAsync(primaryAddress);

        if (!isAddressUpdated)
        {
            _logger.LogError("Could not update address to primary");
            return Result.Error("Could not update address to primary.");
        }

        return Result.Success(_mapper.Map<AddressResponseDto>(addressToBePrimary));
    }
}
