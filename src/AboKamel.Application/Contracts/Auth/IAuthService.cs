using AboKamel.Application.Dtos.Dashboard.Roles;
using Services.Application.Dtos.Authentication;
using Services.Core.DependencyInjection;
using Services.Core.Results;
using Services.Domain.Entities.Users;

namespace Services.Application.Contracts.Auth;

/// <summary>
/// provides an interface for auth-related services that manages auth data across the application. Inherits from IApplicationService and IScopedService.
/// </summary>
public interface IAuthService : IApplicationService, IScopedService
{
    /// <summary>
    /// Logs a user in using the provided login model.
    /// </summary>
    /// <param name="loginRequestDto">The login model containing user credentials.</param>
    /// <returns>A task representing the asynchronous operation, with a result containing login response dto as the login attempt result.</returns>
    Task<ResultAbstract<LoginResponseDto>> Login(LoginRequestDto loginRequestDto);

    /// <summary>
    /// Registers a new user with the specified user details and password, then sends an OTP.
    /// </summary>
    /// <param name="user">The user to register.</param>
    /// <param name="password">The password for the new user.</param>
    /// <returns>A task representing the asynchronous operation, with a result containing application user as the registration attempt result.</returns>
    Task<ResultAbstract<ApplicationUser>> RegisterAsync(ApplicationUser user, string password);

    /// <summary>
    /// Registers a user with the specified user details, password, and role.
    /// </summary>
    /// <param name="user">The user to register.</param>
    /// <param name="password">The password for the new user.</param>
    /// <param name="role">The role to assign to the new user.</param>
    /// <returns>A task representing the asynchronous operation, with a result as the registration attempt result.</returns>
    Task<Result> RegisterUserWithRoleAsync(ApplicationUser user, string password, string role);

    /// <summary>
    /// Updates the details of an existing user.
    /// </summary>
    /// <param name="user">The user with updated details.</param>
    /// <returns>A task representing the asynchronous operation, with a result as the update attempt result.</returns>
    Task<Result> UpdateUserAsync(ApplicationUser user);

    /// <summary>
    /// Get user data.
    /// </summary>
    /// <param name="userId">The user id to retrieve its data.</param>
    /// <returns>A task representing the asynchronous operation, with a result as the update attempt result.</returns>
    Task<ResultAbstract<BaseUserResponseDto>> GetUserAsync(string? userId = null);

    /// <summary>
    /// Changes the password for a specified user.
    /// </summary>
    /// <param name="changePasswordRequest">The change password request DTO.</param>
    /// <param name="userId">The optional user ID (if not provided, uses current user).</param>
    /// <returns>A task representing the asynchronous operation, with a result as the change password attempt result.</returns>
    Task<Result> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequest, string? userId = null);

    /// <summary>
    /// Deletes a specified user.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>A task representing the asynchronous operation, with a result as the delete attempt result.</returns>
    Task<Result> DeleteUserAsync(string userId);

    /// <summary>
    /// Adds a new role to the system.
    /// </summary>
    /// <param name="roleName">The name of the role to be added to the system.</param>
    /// <returns>A task representing the asynchronous operation, with a result as the add role attempt result.</returns>
    Task<Result> CreateRole(string roleName);

    /// <summary>
    /// Adds a user to a specified role.
    /// </summary>
    /// <param name="userId">The ID of the user to be added to the role.</param>
    /// <param name="roleName">The name of the role to add the user to.</param>
    /// <returns>A task representing the asynchronous operation, with a result as the add user to role attempt result.</returns>
    Task<Result> AddUserToRoleAsync(string userId, string roleName);

    Task<ResultAbstract<List<RoleResponseDto>>> GetRolesAsync();

    /// <summary>
    /// Initiates the forget password process and sends an OTP to the user's email.
    /// </summary>
    /// <param name="request">The forget password request DTO.</param>
    /// <returns>A task representing the asynchronous operation, with a result.</returns>
    Task<Result> ForgetPasswordAsync(ForgetPasswordRequestDto request);

    /// <summary>
    /// Verifies the OTP for a specific purpose.
    /// </summary>
    /// <param name="request">The verify OTP request DTO.</param>
    /// <returns>A task representing the asynchronous operation, with a result (possibly with a token or confirmation).</returns>
    Task<ResultAbstract<string>> VerifyOtpAsync(VerifyOtpRequestDto request);

    /// <summary>
    /// Resets the user's password after successful OTP verification.
    /// </summary>
    /// <param name="request">The reset password request DTO.</param>
    /// <returns>A task representing the asynchronous operation, with a result.</returns>
    Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request);

    /// <summary>
    /// Resends an OTP for a specific purpose with rate limiting.
    /// </summary>
    /// <param name="request">The resend OTP request DTO.</param>
    /// <returns>A task representing the asynchronous operation, with a result.</returns>
    Task<Result> ResendOtpAsync(ResendOtpRequestDto request);
}