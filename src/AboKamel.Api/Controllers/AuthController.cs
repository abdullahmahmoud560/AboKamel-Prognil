using AboKamel.Application.Dtos.Dashboard.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Application.Contracts.Auth;
using Services.Application.Dtos.Authentication;
using Services.Core.Results;
using System.Security.Claims;

namespace Services.Api.Controllers;

public class AuthController(IAuthService authService) : BaseController
{
    private readonly IAuthService _authService = authService;

    /// <summary>
    /// action for login a user that take login request dto.
    /// </summary>
    /// <param name="loginModel">The login model.</param>
    /// <returns>result representing of the login successfully.</returns>
    [HttpPost("Login")]
    [ProducesResponseType(typeof(ResultAbstract<LoginResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<ResultAbstract<LoginResponseDto>> Login(LoginRequestDto loginModel)
    {
        var result = await _authService.Login(loginModel);

        if (!result.IsSuccess)
        {
            return result;
        }

        return result;
    }

    /// <summary>
    /// action add a role.
    /// </summary>
    /// <remarks>
    /// access is limited to users with the "Admin" role.
    /// </remarks>
    /// <param name="userId">the user id.</param>
    /// <param name="roleName">the role name.</param>
    /// <returns>result representing the adding the user to the role successfully.</returns>
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("CreateRole/roleName/{roleName}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<Result> AddUserToRole(string roleName)
    {
        return await _authService.CreateRole(roleName);
    }

    /// <summary>
    /// action add a user to a specific role.
    /// </summary>
    /// <remarks>
    /// access is limited to users with the "Admin" role.
    /// </remarks>
    /// <param name="userId">the user id.</param>
    /// <param name="roleName">the role name.</param>
    /// <returns>result representing the adding the user to the role successfully.</returns>
    [Authorize(Roles = "SuperAdmin")]
    [HttpPost("AddUserToRole/userId/{userId}/roleName/{roleName}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<Result> AddUserToRole(string userId, string roleName)
    {
        return await _authService.AddUserToRoleAsync(userId, roleName);
    }

    /// <summary>
    /// action for add an admin account.
    /// </summary>
    /// <returns>result representing the adding admin successfully.</returns>
    [HttpPost("CreateAdminAccount")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<Result> CreateAdminAccount()
    {
        await _authService.CreateAdminAccount();

        return Result.SuccessWithMessage("Create admin account successfully");
    }

    /// <summary>
    /// Deletes a specified user.
    /// </summary>
    /// <remarks>
    /// access is limited to users with the "Admin" role.
    /// </remarks>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>A result representing the attempt to delete the user.</returns>
    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("DeleteUser/{userId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result>> DeleteUser(string userId)
    {
        return await _authService.DeleteUserAsync(userId);
    }

    /// <summary>
    /// Changes the password for a specified user.
    /// </summary>
    /// <param name="changePasswordRequest">The request dto containing information to change the password.</param>
    /// <returns>A result representing the attempt to change the password.</returns>
    [Authorize]
    [HttpPut("ChangePassword")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Result>> ChangePassword(ChangePasswordRequestDto changePasswordRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _authService.ChangePasswordAsync(changePasswordRequest, userId);
    }

    /// <summary>
    /// Get the current logged in user data.
    /// </summary>
    /// <returns>A result containing the current logged in user data.</returns>
    [Authorize]
    [HttpGet("GetCurrentUser")]
    [ProducesResponseType(typeof(ResultAbstract<BaseUserResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResultAbstract<BaseUserResponseDto>>> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return await _authService.GetUserAsync(userId);
    }

    /// <summary>
    /// Get the current logged in user data.
    /// </summary>
    /// <returns>A result containing the current logged in user data.</returns>
    //[Authorize(Roles = "SuperAdmin")]
    [HttpGet("GetRoles")]
    [ProducesResponseType(typeof(ResultAbstract<List<RoleResponseDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResultAbstract<List<RoleResponseDto>>>> GetRolesAsync()
    {
        return await _authService.GetRolesAsync();
    }
}