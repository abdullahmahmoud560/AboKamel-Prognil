using AboKamel.Application.Dtos.Dashboard.Roles;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Application.Contracts.Auth;
using Services.Application.Dtos.Authentication;
using Services.Core.Dtos;
using Services.Core.Results;
using Services.Domain.Entities.Users;
using Services.Infrastructure.DbContexts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Application.Services;

public class AuthService(UserManager<ApplicationUser> userManager, ILogger<ApplicationUser> logger, IMapper mapper, IOptions<JWT> jwt, CapsulaDbContext dbContext, RoleManager<IdentityRole> roleManager = null) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<ApplicationUser> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly JWT _jwt = jwt.Value;
    private readonly CapsulaDbContext _dbContext = dbContext;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    /// <inheritdoc/>
    public async Task<ResultAbstract<LoginResponseDto>> Login(LoginRequestDto LoginRequestDto)
    {
        var user = await _userManager.FindByNameAsync(LoginRequestDto.Email);

        var checkPassword = await _dbContext.Users.Where(u => u.CustomPassword == LoginRequestDto.CustomPassword && u.Id == user.Id).SingleOrDefaultAsync();

        if (user is null || checkPassword  is null/*!await _userManager.CheckPasswordAsync(user, LoginRequestDto.Password)*/)
        {
            if (!await _userManager.CheckPasswordAsync(user, LoginRequestDto.CustomPassword))
            {
                _logger.LogWarning($"Invalid credentials entered for {LoginRequestDto.Email}");
                return Result.Error("Invalid credentials");
            }
        }

        var roles = (List<string>)await _userManager.GetRolesAsync(user);

        var token = GenerateJwtToken(user, roles);

        var loginResponseDto = _mapper.Map<LoginResponseDto>(user);
        loginResponseDto.Roles = roles;
        loginResponseDto.Token = token;

        _logger.LogInformation($"Successfully generated claims for {user.UserName}");
        _logger.LogInformation($"{user.UserName} successfully logged in");
        return ResultAbstract<LoginResponseDto>.Success(loginResponseDto);
    }

    /// <inheritdoc/>
    public async Task<ResultAbstract<ApplicationUser>> RegisterAsync(ApplicationUser user, string password)
    {
        var isUserExist = await _userManager.FindByEmailAsync(user.Email);

        if (isUserExist is not null)
        {
            _logger.LogWarning($"the email is already taken {user.Email}");
            return Result.Error($"the email is already taken {user.Email}");
        }

        string defaultPass = "P@ssw0rd";

        var result = await _userManager.CreateAsync(user, defaultPass);

        if (!result.Succeeded)
        {
            var errors = FormatErrorMessage(result);

            _logger.LogError($"An error occured while creating user: {errors}");
            return Result.Error(errors);
        }

        _logger.LogInformation($"Successfully registered a new user with username {user.UserName}");
        return Result.Success(user);
    }

    /// <inheritdoc/>
    public async Task<Result> UpdateUserAsync(ApplicationUser user)
    {
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = FormatErrorMessage(result);

            _logger.LogError($"An error occured while updating user with username:{user.UserName}, errors: {errors}");
            return Result.Error(errors);
        }

        _logger.LogInformation($"Successfully update user with username {user.UserName}");
        return Result.SuccessWithMessage("Successfully updated the user.");
    }

    /// <inheritdoc/>
    public async Task<Result> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequest, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            _logger.LogError($"Unable to find user with id {userId}");
            return Result.NotFound(["The user is not found"]);
        }

        var result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword);

        if (!result.Succeeded)
        {
            var errors = FormatErrorMessage(result);

            _logger.LogError($"An error occured while change the password for the user with username:{user.UserName}, errors: {errors}");
            return Result.Error(errors);
        }

        _logger.LogInformation($"Successfully changed password for the user with username {user.UserName}");
        return Result.SuccessWithMessage("Successfully changed user password.");
    }

    /// <inheritdoc/>
    public async Task<Result> CreateRole(string roleName)
    {
        var identityRole = new IdentityRole
        {
            Name = roleName,
            NormalizedName = roleName.ToUpper()
        };

        var result = await _roleManager.CreateAsync(identityRole);

        if (!result.Succeeded)
        {
            _logger.LogError($"could not create role {roleName}");
            return Result.Error([$"could not create rle {roleName}"]);
        }

        return Result.SuccessWithMessage($"Succssfully created role {roleName}");
    }

    /// <inheritdoc/>
    public async Task<Result> AddUserToRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            _logger.LogError($"Unable to find user with id {userId} to assign {roleName} role");
            return Result.NotFound(["The user is not found"]);
        }

        var result = await _userManager.AddToRoleAsync(user, roleName);

        if (!result.Succeeded)
        {
            _logger.LogError($"Invalid role name {roleName} while assigning {user.UserName} to the role");
            return Result.Error(["Invalid role name"]);
        }

        _logger.LogInformation($"Successfully assigned {user.UserName} to role {roleName}");
        return Result.SuccessWithMessage($"Successfully assigned to role {roleName}");
    }

    /// <inheritdoc/>
    public async Task<Result> RegisterUserWithRoleAsync(ApplicationUser user, string password, string role)
    {
        var registerResult = await RegisterAsync(user, password);

        if (!registerResult.IsSuccess)
        {
            return Result.Error(registerResult.Errors.SingleOrDefault());
        }

        var addRoleResult = await AddUserToRoleAsync(registerResult.Value.Id, role);

        if (!addRoleResult.IsSuccess)
        {
            return addRoleResult;
        }

        return Result.SuccessWithMessage($"Successfully created a new user {registerResult.Value.UserName} with role {role}");
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            _logger.LogError($"Unable to find user with id {userId} to delete");
            return Result.NotFound(["The user is not found"]);
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            var errors = FormatErrorMessage(result);

            _logger.LogError($"An error occured while deleting the user with username:{user.UserName}, errors: {errors}");
            return Result.Error(errors);
        }

        _logger.LogInformation($"Successfully delete user {user.UserName}");
        return Result.SuccessWithMessage($"Successfully delete , username: {user.UserName}");
    }

    /// <inheritdoc/>
    public async Task CreateAdminAccount()
    {
        var user = new ApplicationUser
        {
            Email = "admin123@gmail.com",
            UserName = "admin",
            PhoneNumber = "01013854588",
            FullName = "Application Super Admin"
        };

        await _userManager.CreateAsync(user, "Admin123!");

        await _userManager.AddToRoleAsync(user, "SuperAdmin");
    }

    /// <summary>
    /// Generates claims for a user.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="roles">The roles.</param>
    /// <returns>The claims identity.</returns>
    public List<Claim> GenerateClaims(ApplicationUser user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    /// <summary>
    /// generates jwt token for a specific user
    /// </summary>
    /// <param name="user">the user to generate jwt token for</param>
    /// <param name="roles">the user's roles</param>
    /// <returns>the jwt security token</returns>
    public string GenerateJwtToken(ApplicationUser user, List<string> roles)
    {
        var claims = GenerateClaims(user, roles);
        Console.WriteLine(_jwt.Key);
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
        issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.DurationInDays),
            signingCredentials: signingCredentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return token;
    }

    /// <summary>
    /// Formats the error messages from an <see cref="IdentityResult"/> into a single string.
    /// </summary>
    /// <param name="result">The <see cref="IdentityResult"/> containing the errors to format.</param>
    /// <returns>A string containing all error descriptions separated by commas.</returns>
    private string FormatErrorMessage(IdentityResult result)
    {
        var errors = string.Empty;

        foreach (var error in result.Errors)
        {
            errors += $"{error.Description},";
        }

        return errors;
    }

    public async Task<ResultAbstract<BaseUserResponseDto>> GetUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            _logger.LogError($"Unable to find user with id {userId} to delete");
            return Result.NotFound(["The user is not found"]);
        }
        var roles = await _userManager.GetRolesAsync(user);

        var userResponseDto = _mapper.Map<BaseUserResponseDto>(user);
        userResponseDto.Role = roles.FirstOrDefault();
        _logger.LogInformation($"Successfully retrieved user {user.UserName}");
        return Result.Success(userResponseDto);
    }

    public async Task<ResultAbstract<List<RoleResponseDto>>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();

        return Result.Success(_mapper.Map<List<RoleResponseDto>>(roles));
    }
}