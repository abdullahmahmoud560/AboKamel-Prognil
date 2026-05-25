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
    /// Registers a new user with the specified user details and password.
    /// </summary>
    /// <param name="user">The user to register.</param>
    /// <param name="password">The password for the new user.</param>
    /// <returns>A task representing the asynchronous operation, with a result containing application user> as the registration attempt result.</returns>
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
    Task<ResultAbstract<BaseUserResponseDto>> GetUserAsync(string userId);

    /// <summary>
    /// Changes the password for a specified user.
    /// </summary>
    /// <param name="userId">The ID of the user whose password is to be changed.</param>
    /// <param name="currentPassword">The current password of the user.</param>
    /// <param name="newPassword">The new password for the user.</param>
    /// <returns>A task representing the asynchronous operation, with a result as the change password attempt result.</returns>
    Task<Result> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequest, string userId);

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
    /// Creates an administrator account with predefined settings.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateAdminAccount();
}