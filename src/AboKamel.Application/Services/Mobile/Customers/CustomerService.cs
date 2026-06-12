using AboKamel.Application.Dtos.Authentication.Users.Customers;
using AboKamel.Core.Dtos;
using AutoMapper;
using Capsula.Application.Contracts.Mobile.Customers;
using Capsula.Application.Dtos.Authentication.Users.Customers;
using Capsula.Domain.Entities.Users.Customers;
using Capsula.Domain.Repositories.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Services.Application.Contracts.Auth;
using Services.Core.Helpers.Roles;
using Services.Core.Results;
using Services.Domain.Entities.Users;
using System.Security.Claims;

namespace Capsula.Application.Services.Mobile.Customers;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly ILogger<Customer> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerService(ICustomerRepository customerRepository, IAuthService authService, IMapper mapper, ILogger<Customer> logger, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _customerRepository = customerRepository;
        _authService = authService;
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ResultAbstract<bool>> EditCustomerStatusAsync(string customerId, bool active)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);

        if (customer is null)
        {
            return Result.Error("العميل غير موجود.");
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

    public async Task<ResultAbstract<CustomerResponseDto>> RegisterCustomerAsync(RegisterCustomerRequestDto request)
    {
        // First, validate password matches confirm password (redundant but adds extra layer of safety in service layer)
        if (request.Password != request.ConfirmPassword)
        {
            return Result.Error("كلمة المرور وتأكيد كلمة المرور غير متطابقتين");
        }

        var role = RoleName.Customer;

        // Create customer manually (or use AutoMapper after updating mappings
        var customer = new Customer
        {
            UserName = request.Email, // Use email as username
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            FullName = request.FullName,
            EstablishmentName = request.EstablishmentName,
            EstablishmentType = request.EstablishmentType,
            Address = request.Address,
            Landmark = request.Landmark,
            AreaId = request.AreaId
        };

        var customerAdded = await _authService.RegisterUserWithRoleAsync(customer, request.Password, role);

        if (!customerAdded.IsSuccess)
        {
            return Result.Error(customerAdded.Errors.FirstOrDefault() ?? "حدث خطأ");
        }

        _logger.LogInformation("تم إضافة العميل إلى قاعدة البيانات بنجاح");
        return Result.Success(_mapper.Map<CustomerResponseDto>(customer));
    }

    public async Task<ResultAbstract<CustomerResponseDto>> UpdateCustomerAsync(string userId, UpdateCustomerRequestDto dto)
    {
        // Find customer by ID
        var customer = await _userManager.FindByIdAsync(userId) as Customer;

        if (customer == null)
        {
            return Result.Error("العميل غير موجود.");
        }

        // Update only the provided fields
        if (!string.IsNullOrEmpty(dto.FullName))
            customer.FullName = dto.FullName;

        if (!string.IsNullOrEmpty(dto.Email))
        {
            customer.Email = dto.Email;
            customer.UserName = dto.Email; // Update UserName to match Email if desired
        }

        if (!string.IsNullOrEmpty(dto.PhoneNumber))
            customer.PhoneNumber = dto.PhoneNumber;

        if (!string.IsNullOrEmpty(dto.Address))
            customer.Address = dto.Address;

        if (!string.IsNullOrEmpty(dto.Landmark))
            customer.Landmark = dto.Landmark;

        if (!string.IsNullOrEmpty(dto.EstablishmentName))
            customer.EstablishmentName = dto.EstablishmentName;

        if (!string.IsNullOrEmpty(dto.EstablishmentType))
            customer.EstablishmentType = dto.EstablishmentType;

        if (dto.AreaId > 0)
            customer.AreaId = dto.AreaId;

        // Update user
        var updateResult = await _userManager.UpdateAsync(customer);

        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            _logger.LogError($"فشل تحديث العميل: {errors}");
            return Result.Error($"فشل تحديث العميل: {errors}");
        }

        _logger.LogInformation($"العميل رقم {userId} تم تحديثه بنجاح");
        return Result.Success(_mapper.Map<CustomerResponseDto>(customer));
    }

    // Backward compatibility overload for Dashboard
    public async Task<ResultAbstract<CustomerResponseDto>> UpdateCustomerAsync(CustomerRequestDto request, string? customerId = null)
    {
        // Find customer by ID or use current user ID if not provided
        var id = customerId ?? GetCurrentUserId();
        var customer = await _userManager.FindByIdAsync(id) as Customer;

        if (customer == null)
        {
            return Result.Error("العميل غير موجود.");
        }

        // Map properties from the old CustomerRequestDto
        _mapper.Map(request, customer);

        // Update user
        var updateResult = await _userManager.UpdateAsync(customer);

        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            _logger.LogError($"فشل تحديث العميل: {errors}");
            return Result.Error($"فشل تحديث العميل: {errors}");
        }

        _logger.LogInformation($"العميل رقم {id} تم تحديثه بنجاح (backward compatible)");
        return Result.Success(_mapper.Map<CustomerResponseDto>(customer));
    }

    public async Task<Result> DeleteCustomerAccountAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result.NotFound("المستخدم غير موجود.");
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Error(errors);
        }

        return Result.SuccessWithMessage("تم حذف الحساب بنجاح.");
    }

    // Helper method to get current user ID
    private string GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("المستخدم غير مصادق عليه.");
        }
        return userId;
    }
}
