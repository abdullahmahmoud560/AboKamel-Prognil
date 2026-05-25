using AboKamel.Core.Dtos;
using AutoMapper;
using Capsula.Application.Contracts.Mobile.Customers;
using Capsula.Application.Dtos.Authentication.Users.Customers;
using Capsula.Domain.Entities.Users.Customers;
using Capsula.Domain.Repositories.Customers;
using Microsoft.Extensions.Logging;
using Services.Application.Contracts.Auth;
using Services.Core.Helpers.Roles;
using Services.Core.Results;

namespace Capsula.Application.Services.Mobile.Customers;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly ILogger<Customer> _logger;

    public CustomerService(ICustomerRepository customerRepository, IAuthService authService, IMapper mapper, ILogger<Customer> logger)
    {
        _customerRepository = customerRepository;
        _authService = authService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ResultAbstract<bool>> EditCustomerStatusAsync(string customerId, bool active)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);

        if (customer is null)
        {
            return Result.Error("Customer does not exist.");
        }

        customer.Active = active;
        await _customerRepository.EditAsync(customer);

        return Result.Success(true);
    }

    public async Task<ResultAbstract<List<CustomerWithRolesDto>>> GetAllCustomerAsync()
    {
        var customers = await _customerRepository.GetAllCustomersWithRolesAsync();
        return Result.Success(customers);
    }

    public async Task<ResultAbstract<CustomerResponseDto>> RegisterCustomerAsync(CustomerRequestDto request)
    {
        var role = RoleName.Customer;

        var customer = _mapper.Map<Customer>(request);
        var customerAdded = await _authService.RegisterUserWithRoleAsync(customer, request.CustomPassword, role);

        if (!customerAdded.IsSuccess)
        {
            return Result.Error(customerAdded.Errors.FirstOrDefault());
        }

        _logger.LogInformation("customer added successfully to the database");
        return Result.Success(_mapper.Map<CustomerResponseDto>(customer));
    }

    public async Task<ResultAbstract<CustomerResponseDto>> UpdateCustomerAsync(CustomerRequestDto request, string customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);

        if(customer is null)
        {
            return Result.Error("Customer does not exist.");
        }

        _mapper.Map(request, customer);

        await _authService.UpdateUserAsync(customer);

        return _mapper.Map<CustomerResponseDto>(customer);
    }
}
