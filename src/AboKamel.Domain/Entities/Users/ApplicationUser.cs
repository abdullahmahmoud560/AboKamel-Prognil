using Microsoft.AspNetCore.Identity;
using Services.Core.Entities;

namespace Services.Domain.Entities.Users;

public class ApplicationUser : IdentityUser , IEntity<string>
{
    public string FullName { get; set; } = string.Empty;
    public string CustomPassword { get; set; } = string.Empty;
}
