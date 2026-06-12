using Services.Core.Entities;
using System;

namespace Services.Domain.Entities.Users;

public class TwoFactorVerify : IEntity<int>
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string OTPHash { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty; // like "Register" or "ResetPassword"
    public DateTime ExpirationDate { get; set; }
    public int FailedAttempts { get; set; }
    public bool IsVerified { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;
}
