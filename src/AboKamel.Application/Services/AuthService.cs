using AboKamel.Application.Dtos.Dashboard.Roles;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Application.Contracts.Auth;
using Services.Application.Dtos.Authentication;
using Services.Core.Contracts;
using Services.Core.Dtos;
using Services.Core.Results;
using Services.Domain.Entities.Users;
using Services.Infrastructure.DbContexts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ApplicationUser> _logger;
    private readonly IMapper _mapper;
    private readonly JWT _jwt;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailService _emailService;
    private readonly CapsulaDbContext _dbContext;

    public AuthService(UserManager<ApplicationUser> userManager, ILogger<ApplicationUser> logger, IMapper mapper, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor, IEmailService emailService, CapsulaDbContext dbContext)
    {
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _jwt = jwt.Value;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
        _emailService = emailService;
        _dbContext = dbContext;
    }

    private string GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException("المستخدم غير مصرح له.");
        return userId;
    }

    public async Task<ResultAbstract<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, loginRequestDto.Password))
        {
            return Result.Error("بيانات الدخول غير صحيحة");
        }

        if (!user.EmailConfirmed)
        {
            return Result.Error("لم يتم تفعيل حسابك بعد. يرجى التحقق من بريدك الإلكتروني أولاً.");
        }

        var roles = (List<string>)await _userManager.GetRolesAsync(user);
        var token = GenerateJwtToken(user, roles);
        var loginResponseDto = _mapper.Map<LoginResponseDto>(user);
        loginResponseDto.Roles = roles;
        loginResponseDto.Token = token;

        return ResultAbstract<LoginResponseDto>.Success(loginResponseDto);
    }

    public async Task<ResultAbstract<ApplicationUser>> RegisterAsync(ApplicationUser user, string password)
    {
        if (string.IsNullOrEmpty(user.Email)) return Result.Error("البريد الإلكتروني مطلوب");
        var isUserExist = await _userManager.FindByEmailAsync(user.Email);
        if (isUserExist is not null) return Result.Error("هذا البريد الإلكتروني مسجل مسبقاً");

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded) return Result.Error(FormatErrorMessage(result));

        // Generate and send OTP
        await GenerateAndSendOtpAsync(user.Id, user.Email, "Register");

        return Result.Success(user);
    }

    public async Task<Result> UpdateUserAsync(ApplicationUser user)
    {
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return Result.Error(FormatErrorMessage(result));
        return Result.SuccessWithMessage("تم تحديث بيانات المستخدم بنجاح.");
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordRequestDto changePasswordRequest, string? userId = null)
    {
        if (changePasswordRequest.NewPassword != changePasswordRequest.ConfirmPassword)
            return Result.Error("كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقتين");

        var currentUserId = userId ?? GetCurrentUserId();
        var user = await _userManager.FindByIdAsync(currentUserId);
        if (user is null) return Result.NotFound(["المستخدم غير موجود"]);

        var result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword);
        if (!result.Succeeded) return Result.Error(FormatErrorMessage(result));

        return Result.SuccessWithMessage("تم تغيير كلمة المرور بنجاح.");
    }

    public async Task<Result> CreateRole(string roleName)
    {
        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        if (!result.Succeeded) return Result.Error(["تعذر إنشاء الدور"]);
        return Result.SuccessWithMessage("تم إنشاء الدور بنجاح");
    }

    public async Task<Result> AddUserToRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return Result.NotFound(["المستخدم غير موجود"]);
        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (!result.Succeeded) return Result.Error(["اسم الدور غير صحيح"]);
        return Result.SuccessWithMessage("تم إضافة المستخدم للدور بنجاح");
    }

    public async Task<Result> RegisterUserWithRoleAsync(ApplicationUser user, string password, string role)
    {
        var registerResult = await RegisterAsync(user, password);
        if (!registerResult.IsSuccess) return Result.Error(registerResult.Errors.FirstOrDefault() ?? "حدث خطأ");
        var addRoleResult = await AddUserToRoleAsync(registerResult.Value.Id, role);
        return addRoleResult;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return Result.NotFound(["المستخدم غير موجود"]);
        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) return Result.Error(FormatErrorMessage(result));
        return Result.SuccessWithMessage("تم حذف المستخدم بنجاح");
    }

    public async Task<ResultAbstract<BaseUserResponseDto>> GetUserAsync(string? userId = null)
    {
        var currentUserId = userId ?? GetCurrentUserId();
        var user = await _userManager.FindByIdAsync(currentUserId);
        if (user is null) return Result.NotFound(["المستخدم غير موجود"]);
        var roles = await _userManager.GetRolesAsync(user);
        var userResponseDto = _mapper.Map<BaseUserResponseDto>(user);
        userResponseDto.Role = roles.FirstOrDefault() ?? string.Empty;
        return Result.Success(userResponseDto);
    }

    public async Task<ResultAbstract<List<RoleResponseDto>>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return Result.Success(_mapper.Map<List<RoleResponseDto>>(roles));
    }

    public string GenerateJwtToken(ApplicationUser user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
        };
        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(_jwt.Issuer, _jwt.Audience, claims, expires: DateTime.Now.AddDays(_jwt.DurationInDays), signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string FormatErrorMessage(IdentityResult result) => string.Join(", ", result.Errors.Select(e => e.Description));

    private string GenerateOtp()
    {
        return new Random().Next(100000, 999999).ToString();
    }

    private string HashOtp(string otp)
    {
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(otp));
            return Convert.ToBase64String(bytes);
        }
    }

    private async Task GenerateAndSendOtpAsync(string userId, string email, string purpose)
    {
        var otp = GenerateOtp();
        var otpHash = HashOtp(otp);

        // Remove old OTPs for this user and purpose
        var oldOtps = await _dbContext.TwoFactorVerifies
            .Where(x => x.UserId == userId && x.Purpose == purpose)
            .ToListAsync();
        _dbContext.TwoFactorVerifies.RemoveRange(oldOtps);

        var twoFactorVerify = new TwoFactorVerify
        {
            UserId = userId,
            Email = email,
            OTPHash = otpHash,
            Purpose = purpose,
            ExpirationDate = DateTime.UtcNow.AddMinutes(5),
            FailedAttempts = 0,
            IsVerified = false
        };

        await _dbContext.TwoFactorVerifies.AddAsync(twoFactorVerify);
        await _dbContext.SaveChangesAsync();

        // Send OTP via email
        var subject = purpose == "Register" ? "تأكيد التسجيل" : "إعادة تعيين كلمة المرور";
        var body = purpose == "Register"
            ? $"مرحبًا،\n\nرمز التأكيد الخاص بك هو: {otp}\n\nينتهي صلاحيته خلال 5 دقائق."
            : $"مرحبًا،\n\nرمز إعادة تعيين كلمة المرور الخاص بك هو: {otp}\n\nينتهي صلاحيته خلال 5 دقائق.";

        await _emailService.SendEmailAsync(email, subject, body);
    }

    public async Task<Result> ForgetPasswordAsync(ForgetPasswordRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is not null)
        {
            await GenerateAndSendOtpAsync(user.Id, user.Email, "ResetPassword");
        }

        return Result.SuccessWithMessage("تم إرسال رمز التحقق إلى البريد الخاص بك.");
    }

    public async Task<ResultAbstract<string>> VerifyOtpAsync(VerifyOtpRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null) return Result.NotFound(["المستخدم غير موجود"]);

        var twoFactorVerify = await _dbContext.TwoFactorVerifies
            .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Purpose == request.Purpose && !x.IsVerified);

        if (twoFactorVerify is null) return Result.Error("لم يتم العثور على رمز التأكيد");

        if (twoFactorVerify.ExpirationDate < DateTime.UtcNow)
        {
            return Result.Error("انتهت صلاحية رمز التأكيد");
        }

        if (twoFactorVerify.FailedAttempts >= 5)
        {
            return Result.Error("تم تجاوز عدد محاولات التحقق");
        }

        var otpHash = HashOtp(request.OTP);
        if (twoFactorVerify.OTPHash != otpHash)
        {
            twoFactorVerify.FailedAttempts++;
            await _dbContext.SaveChangesAsync();
            return Result.Error("رمز التأكيد غير صحيح");
        }

        twoFactorVerify.IsVerified = true;
        await _dbContext.SaveChangesAsync();

        // If verifying registration, mark email as confirmed
        if (request.Purpose == "Register")
        {
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
        }

        // Generate a temporary token (could be a JWT or just a confirmation, but let's return a simple success message)
        return ResultAbstract<string>.Success("تم التحقق بنجاح من رمز التأكيد");
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequestDto request)
    {
        if (request.NewPassword != request.ConfirmPassword)
            return Result.Error("كلمة المرور الجديدة وتأكيدها غير متطابقتين");

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null) return Result.NotFound(["المستخدم غير موجود"]);

        // Check if there's a verified OTP
        var twoFactorVerify = await _dbContext.TwoFactorVerifies
            .FirstOrDefaultAsync(x => x.UserId == user.Id && x.Purpose == "ResetPassword" && x.IsVerified && x.ExpirationDate >= DateTime.UtcNow);
        if (twoFactorVerify is null) return Result.Error("لم يتم التحقق من رمز التأكيد أو انتهت صلاحيته");

        // Reset password
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
        if (!result.Succeeded) return Result.Error(FormatErrorMessage(result));

        // Remove used OTP
        _dbContext.TwoFactorVerifies.Remove(twoFactorVerify);
        await _dbContext.SaveChangesAsync();

        return Result.SuccessWithMessage("تم إعادة تعيين كلمة المرور بنجاح");
    }
}
